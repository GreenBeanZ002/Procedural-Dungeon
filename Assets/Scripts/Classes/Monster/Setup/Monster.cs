using System.Collections.Generic;

using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Stats")]
    [SerializeField]
    protected string MONSTER_NAME;
    public string Monster_Name => MONSTER_NAME;

    [SerializeField]
    protected int MONSTER_HEALTH;
    public int Monster_Health => MONSTER_HEALTH;

    [SerializeField]
    protected float MONSTER_SPEED = 5f;
    public float Monster_Speed => MONSTER_SPEED;

    [SerializeField]
    protected int MONSTER_DAMAGE;
    public int Monster_Damage => MONSTER_DAMAGE;

    [SerializeField]
    protected Sprite MONSTER_SPRITE;
    public Sprite Monster_Sprite => MONSTER_SPRITE;

    [SerializeField]
    protected AudioSource MONSTER_HURT;

    public AudioSource MonsterHurt => MONSTER_HURT;

    [Header("Detection & Pathfinding")]
    [SerializeField] protected Transform player;
    [SerializeField] protected float detectionRange = 12f;
    [SerializeField] protected float loseRange = 8f;
    [SerializeField] protected float stopRange = 3f;
    [SerializeField] protected float repathInterval = 0.5f;
    [SerializeField] protected float tileSize = 0.5f;

    [Header("Flee Behavior")]
    [SerializeField] protected bool canFlee = true;
    [SerializeField] protected float fleeRange = 1.5f;       
    [SerializeField] protected float fleeDelay = 0.5f;      
    [SerializeField] protected float fleeExitRange = 3f;      
    [SerializeField] protected float fleeDistanceMin = 5f;    
    [SerializeField] protected float fleeDistanceMax = 15f;   

    [Header("Other")]
    private DropTable dropTable;
    private SpriteRenderer spriteRenderer;
    private PathfindingUtils pathfinder;

    private List<Vector2Int> path;
    private int pathIndex;
    private float repathTimer;
    private bool isChasing = false;

    private float closeTimer = 0f;
    private bool isFleeing = false;
    private float currentFleeDistance;

    protected virtual void Awake()
    {

        SetDefaults();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dropTable = GetComponent<DropTable>();
        pathfinder = GetComponent<PathfindingUtils>();
        SetupSprite();

        player = GameObject.FindGameObjectWithTag("player").transform;

    }

    protected virtual void Update()
    {
        if (player == null) return;

        float sqrDist = (player.position - transform.position).sqrMagnitude;

        if (canFlee)
        {
            if (sqrDist <= fleeRange * fleeRange)
            {
                closeTimer += Time.deltaTime;
                if (!isFleeing && closeTimer >= fleeDelay)
                {
                    isFleeing = true;
                    isChasing = false;
                    currentFleeDistance = Random.Range(fleeDistanceMin, fleeDistanceMax);

                }
            }
            else
            {
                closeTimer = 0f;

                if (isFleeing && sqrDist > fleeExitRange * fleeExitRange)
                {
                    isFleeing = false;
                }
            }
        }

        if (isFleeing)
        {
            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                RepathFlee();
                repathTimer = repathInterval;
            }

            FollowPath();
            return; 
        }

        if (!isChasing)
        {
            if (sqrDist <= detectionRange * detectionRange)
            {
                isChasing = true;
                repathTimer = 0f;
            }
            else
            {
            }
        }
        else
        {
            if (sqrDist > loseRange * loseRange)
            {
                isChasing = false;
                path = null;
                return;
            }
        }

        bool withinStopRange = sqrDist <= stopRange * stopRange;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            Repath();
            repathTimer = repathInterval;
        }

        if (!withinStopRange)
        {
            FollowPath();
        }

    }

    private void Repath()
    {
        if (pathfinder == null)
        {
            Debug.LogWarning("pathfinder is null!");
            return;
        }

        var walkable = pathfinder.GetWalkableTiles();

        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int playerGrid = WorldToGrid(player.position);

        var newPath = PathfindingUtils.FindPath(start, playerGrid, walkable);
        if (newPath == null || newPath.Count == 0) return; 

        int trim = Mathf.Max(0, Mathf.RoundToInt(stopRange / tileSize));
        if (trim > 0 && newPath.Count > trim)
            newPath.RemoveRange(newPath.Count - trim, trim);

        path = newPath;
        pathIndex = 0;
    }

    private void RepathFlee()
    {
        if (pathfinder == null)
        {
            Debug.LogWarning("pathfinder is null!");
            return;
        }

        var walkable = pathfinder.GetWalkableTiles();

        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int playerGrid = WorldToGrid(player.position);
        Vector2Int target = GetFleeTile(playerGrid, start, walkable);

        var newPath = PathfindingUtils.FindPath(start, target, walkable);
        if (newPath == null || newPath.Count == 0) return; 

        path = newPath;
        pathIndex = 0;
    }

    private Vector2Int GetFleeTile(Vector2Int playerGrid, Vector2Int myGrid, HashSet<Vector2Int> walkable)
    {
        Vector2 dir = (Vector2)(myGrid - playerGrid);
        if (dir.sqrMagnitude < 0.01f) dir = Random.insideUnitCircle.normalized;
        dir.Normalize();

        int fleeTiles = Mathf.Max(1, Mathf.RoundToInt(currentFleeDistance / tileSize));


        float[] angleOffsetsDeg = { 0f, 20f, -20f, 40f, -40f, 60f, -60f, 90f, -90f };

        float currentSqrDistToPlayer = ((Vector2)(myGrid - playerGrid)).sqrMagnitude;

        foreach (float angleDeg in angleOffsetsDeg)
        {
            Vector2 rotatedDir = Rotate(dir, angleDeg);
            Vector2Int offset = new Vector2Int(
                Mathf.RoundToInt(rotatedDir.x * fleeTiles),
                Mathf.RoundToInt(rotatedDir.y * fleeTiles)
            );
            Vector2Int candidate = myGrid + offset;

            if (!walkable.Contains(candidate)) continue;


            float candidateSqrDist = ((Vector2)(candidate - playerGrid)).sqrMagnitude;
            if (candidateSqrDist < currentSqrDistToPlayer) continue;

            return candidate;
        }


        for (int shortenedTiles = fleeTiles - 1; shortenedTiles >= 1; shortenedTiles--)
        {
            foreach (float angleDeg in angleOffsetsDeg)
            {
                Vector2 rotatedDir = Rotate(dir, angleDeg);
                Vector2Int offset = new Vector2Int(
                    Mathf.RoundToInt(rotatedDir.x * shortenedTiles),
                    Mathf.RoundToInt(rotatedDir.y * shortenedTiles)
                );
                Vector2Int candidate = myGrid + offset;

                if (!walkable.Contains(candidate)) continue;

                float candidateSqrDist = ((Vector2)(candidate - playerGrid)).sqrMagnitude;
                if (candidateSqrDist < currentSqrDistToPlayer) continue;

                return candidate;
            }
        }

        return myGrid;
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    private void FollowPath()
    {
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetPos = GridToWorld(path[pathIndex]);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MONSTER_SPEED * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            pathIndex++;
    }

    private Vector2Int WorldToGrid(Vector3 worldPos) =>
        new Vector2Int(Mathf.RoundToInt(worldPos.x / tileSize), Mathf.RoundToInt(worldPos.y / tileSize));

    private Vector3 GridToWorld(Vector2Int gridPos) =>
        new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, -0.2f);

    protected void SetupSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = MONSTER_SPRITE;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        MONSTER_HEALTH -= damage;
        if (MONSTER_HEALTH <= 0)
        {
            HandleDeath();
        }
        Debug.Log("Ow I took damage! Current health: " + MONSTER_HEALTH);
    }

    protected virtual void HandleDeath()
    {
        dropTable.DropAll(transform.position);
        Destroy(gameObject);
    }

    protected abstract void SetDefaults();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, fleeRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("playerAttack"))
        {
            Player player = FindObjectOfType<Player>();
            TakeDamage(player.GetDamage());
            Destroy(collision.collider.gameObject);
        }
    }
}