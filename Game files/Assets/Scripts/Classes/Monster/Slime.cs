using UnityEngine;

public class Slime : Monster
{
    [SerializeField] private Sprite monsterSprite;

    [Header("Hop Behavior")]
    [SerializeField] private float hopDuration = 0.35f;   // time spent moving per hop
    [SerializeField] private float pauseDuration = 0.6f;  // time spent squatting between hops while chasing
    [SerializeField] private float hopHeight = 0.25f;     // bounce arc (only applies if the sprite is on a child object)

    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 3f;           // how far from its current spot it picks a new destination
    [SerializeField] private float wanderPauseMultiplier = 2f;  // wandering rests are this much longer than chase rests

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;      // slime stops hopping and attacks inside this range
    [SerializeField] private float attackCooldown = 1f;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Order: [0] = resting/squished, then the hop frames in order (e.g. launch, land)
    [SerializeField] private Sprite[] frames;

    private bool isHopping = false;
    private float stateTimer;
    private float attackTimer = 0.5f;
    private Transform visual;
    private Vector3 visualBasePos;

    protected override void SetDefaults()
    {
        MONSTER_NAME = "Slime";
        MONSTER_HEALTH = 8;
        MONSTER_SPEED = 3f;   // speed while mid-hop; hop distance is roughly MONSTER_SPEED * hopDuration
        MONSTER_DAMAGE = 5;
        MONSTER_SPRITE = monsterSprite;
    }

    protected override void Awake()
    {
        base.Awake();

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // The vertical arc only works if the sprite is on a child object.
        // On the root it would fight with FollowPath, so it's skipped.
        if (spriteRenderer != null && spriteRenderer.transform != transform)
        {
            visual = spriteRenderer.transform;
            visualBasePos = visual.localPosition;
        }

        // Random start delay so a group of slimes doesn't hop in sync
        stateTimer = Random.Range(0f, pauseDuration);
    }

    protected override bool HandleCustomBehavior()
    {
        float sqrDist = (player.position - transform.position).sqrMagnitude;

        // Detection (same ranges as Monster, but lose range can never be smaller than detection range)
        float loseRange = Mathf.Max(LOSE_RANGE, DETECTION_RANGE);
        if (!isChasing)
        {
            if (sqrDist > DETECTION_RANGE * DETECTION_RANGE)
            {
                // Can't see the player: hop around randomly
                UpdateHopping(false);
                return true;
            }
            isChasing = true;
        }
        else if (sqrDist > loseRange * loseRange)
        {
            // Lost the player: drop the chase path and start wandering
            isChasing = false;
            isHopping = false;
            stateTimer = pauseDuration;
            path = null;
            return true;
        }

        // Close enough: stop hopping and hit the player
        if (sqrDist <= attackRange * attackRange)
        {
            isHopping = false;
            attackPlayer();
            return true;
        }

        UpdateHopping(true);
        return true;
    }

    // Shared hop/rest cycle. "chasing" decides where the slime is heading.
    private void UpdateHopping(bool chasing)
    {
        stateTimer -= Time.deltaTime;

        if (isHopping)
        {
            FollowPath(); // only moves while mid-hop

            if (stateTimer <= 0f)
            {
                isHopping = false;
                stateTimer = chasing
                    ? pauseDuration
                    : pauseDuration * wanderPauseMultiplier * Random.Range(0.75f, 1.5f);
            }
        }
        else if (stateTimer <= 0f)
        {
            bool hasPath = chasing ? RepathChase() : PickWanderPath();

            if (hasPath)
            {
                isHopping = true;
                stateTimer = hopDuration;
            }
            else
            {
                stateTimer = pauseDuration; // nowhere to go, try again after a rest
            }
        }
    }

    private bool RepathChase()
    {
        if (pathfinder == null)
        {
            Debug.LogWarning("pathfinder is null!");
            return false;
        }

        var walkable = pathfinder.GetWalkableTiles();

        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int target = WorldToGrid(player.position);

        var newPath = PathfindingUtils.FindPath(start, target, walkable);
        if (newPath == null || newPath.Count == 0) return false;

        // Don't hop onto the player's exact tile
        if (newPath.Count > 1) newPath.RemoveAt(newPath.Count - 1);

        path = newPath;
        pathIndex = 0;
        return true;
    }

    // Keeps hopping along the current wander path; picks a new random destination once it's finished
    private bool PickWanderPath()
    {
        if (path != null && pathIndex < path.Count) return true;

        if (pathfinder == null)
        {
            Debug.LogWarning("pathfinder is null!");
            return false;
        }

        var walkable = pathfinder.GetWalkableTiles();
        Vector2Int start = WorldToGrid(transform.position);
        float radiusTiles = Mathf.Max(1f, wanderRadius / TILE_SIZE);

        for (int attempt = 0; attempt < 8; attempt++)
        {
            Vector2 offset = Random.insideUnitCircle * radiusTiles;
            Vector2Int candidate = start + new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));

            if (candidate == start || !walkable.Contains(candidate)) continue;

            var newPath = PathfindingUtils.FindPath(start, candidate, walkable);
            if (newPath == null || newPath.Count == 0) continue;

            path = newPath;
            pathIndex = 0;
            return true;
        }

        return false;
    }

    protected override void animateMonster()
    {
        // 0 at launch, 1 at landing
        float progress = isHopping ? 1f - Mathf.Clamp01(stateTimer / hopDuration) : 0f;

        if (visual != null)
        {
            float arc = isHopping ? Mathf.Sin(progress * Mathf.PI) * hopHeight : 0f;
            visual.localPosition = visualBasePos + Vector3.up * arc;
        }

        // Squished while resting, then step through the remaining frames during the hop
        if (spriteRenderer != null && frames != null && frames.Length > 0)
        {
            int index = 0;
            if (isHopping && frames.Length > 1)
            {
                index = 1 + Mathf.Min(frames.Length - 2, Mathf.FloorToInt(progress * (frames.Length - 1)));
            }
            spriteRenderer.sprite = frames[index];
        }
    }

    protected override void attackPlayer()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer > 0f) return;

        HealthScript.RemoveHealth(MONSTER_DAMAGE);
        attackTimer = attackCooldown;
    }

    protected override void OnDrawGizmosSelectedExtra()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}