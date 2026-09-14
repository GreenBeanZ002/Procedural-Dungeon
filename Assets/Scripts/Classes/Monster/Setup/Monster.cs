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
    [SerializeField] protected float detectionRange = 6f;
    [SerializeField] protected float loseRange = 8f;
    [SerializeField] protected float stopRange = 3f;
    [SerializeField] protected float repathInterval = 0.5f;
    [SerializeField] protected float tileSize = 0.5f;

    [Header("Other")]
    private DropTable dropTable;
    private SpriteRenderer spriteRenderer;
    private PathfindingUtils pathfinder;

    private List<Vector2Int> path;
    private int pathIndex;
    private float repathTimer;
    private bool isChasing = false;

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

        if (!isChasing)
        {
            if (sqrDist <= detectionRange * detectionRange)
            {
                isChasing = true;
                repathTimer = 0f;
            }
            else
            {
                return; 
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
        Vector2Int target = GetStandoffTile(playerGrid, start, walkable);

        path = PathfindingUtils.FindPath(start, target, walkable);
        pathIndex = 0;
    }

    private Vector2Int GetStandoffTile(Vector2Int playerGrid, Vector2Int myGrid, HashSet<Vector2Int> walkable)
    {
        Vector2 dir = (Vector2)(myGrid - playerGrid);
        if (dir.sqrMagnitude < 0.01f) dir = Vector2.right;
        dir.Normalize();

        int standoffTiles = Mathf.Max(1, Mathf.RoundToInt(stopRange / tileSize));
        Vector2Int offset = new Vector2Int(Mathf.RoundToInt(dir.x * standoffTiles), Mathf.RoundToInt(dir.y * standoffTiles));
        Vector2Int candidate = playerGrid + offset;

        return walkable.Contains(candidate) ? candidate : playerGrid;
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
