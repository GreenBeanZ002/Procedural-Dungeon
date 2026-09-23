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
    [SerializeField]
    protected float DETECTION_RANGE = 12f;

    public float DetectionRange => DETECTION_RANGE;

    [SerializeField]
    protected float LOSE_RANGE = 8f;
    public float LoseRange => LOSE_RANGE;

    [SerializeField]
    protected float STOP_RANGE = 3f;
    public float StopRange => STOP_RANGE;

    [SerializeField]
    protected float REPATH_INTERVAL = 0.5f;
    public float RepathInterval => REPATH_INTERVAL;

    [SerializeField]
    protected float TILE_SIZE = 0.5f;
    public float TileSize => TILE_SIZE; 


    [SerializeField] protected Transform player;

    [Header("Other")]
    private DropTable dropTable;
    private SpriteRenderer spriteRenderer;

    protected PathfindingUtils pathfinder;
    protected List<Vector2Int> path;
    protected int pathIndex;
    protected float repathTimer;
    protected bool isChasing = false;

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
        animateMonster();
        if (player == null) return;

        if (HandleCustomBehavior()) return;

        if (SkipDefaultChase) return;

        float sqrDist = (player.position - transform.position).sqrMagnitude;

        if (!isChasing)
        {
            if (sqrDist <= DETECTION_RANGE * DETECTION_RANGE)
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
            if (sqrDist > LOSE_RANGE * LOSE_RANGE)
            {
                isChasing = false;
                path = null;
                return;
            }
        }

        bool withinStopRange = sqrDist <= STOP_RANGE * STOP_RANGE;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            Repath();
            repathTimer = REPATH_INTERVAL;
        }

        if (!withinStopRange)
        {
            FollowPath();
        }
        if (withinStopRange)
        {
            attackPlayer();
        }


    }
    protected abstract void attackPlayer();


    protected virtual bool HandleCustomBehavior()
    {
        return false;
    }

    protected virtual bool SkipDefaultChase => false;

    protected void Repath()
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

        int trim = Mathf.Max(0, Mathf.RoundToInt(STOP_RANGE / TILE_SIZE));
        if (trim > 0 && newPath.Count > trim)
            newPath.RemoveRange(newPath.Count - trim, trim);

        path = newPath;
        pathIndex = 0;
    }

    protected void FollowPath()
    {
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetPos = GridToWorld(path[pathIndex]);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MONSTER_SPEED * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            pathIndex++;
    }

    protected Vector2Int WorldToGrid(Vector3 worldPos) =>
        new Vector2Int(Mathf.RoundToInt(worldPos.x / TILE_SIZE), Mathf.RoundToInt(worldPos.y / TILE_SIZE));

    protected Vector3 GridToWorld(Vector2Int gridPos) =>
        new Vector3(gridPos.x * TILE_SIZE, gridPos.y * TILE_SIZE, -0.2f);

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

    protected abstract void animateMonster();

    protected abstract void SetDefaults();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DETECTION_RANGE);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LOSE_RANGE);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, STOP_RANGE);

        OnDrawGizmosSelectedExtra();
    }

    protected virtual void OnDrawGizmosSelectedExtra() { }

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