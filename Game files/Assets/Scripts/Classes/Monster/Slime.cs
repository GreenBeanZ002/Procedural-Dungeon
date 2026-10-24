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

    [Header("Lunge Attack")]
    [SerializeField] private float lungeRange = 2f;             // within this distance the next hop becomes a lunge at the player
    [SerializeField] private float lungeSpeedMultiplier = 2f;   // lunge hop speed relative to normal MONSTER_SPEED
    [SerializeField] private float lungeDuration = 0.3f;        // lunge hop length
    [SerializeField] private float lungeRecovery = 1f;          // rest after a lunge or after hitting the player
    [SerializeField] private float contactCooldown = 1f;        // minimum time between damage ticks

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Order: [0] = resting/squished, then the hop frames in order (e.g. launch, land)
    [SerializeField] private Sprite[] frames;

    private bool isHopping = false;
    private bool isLunging = false;
    private float stateTimer;
    private float currentHopDuration;
    private float baseSpeed;
    private float nextContactDamageTime = 0f;
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

        baseSpeed = MONSTER_SPEED; // SetDefaults has run by now
        currentHopDuration = hopDuration;

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
            isLunging = false;
            MONSTER_SPEED = baseSpeed;
            stateTimer = pauseDuration;
            path = null;
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
                float rest = isLunging
                    ? lungeRecovery
                    : (chasing ? pauseDuration : pauseDuration * wanderPauseMultiplier * Random.Range(0.75f, 1.5f));
                EndHop(rest);
            }
        }
        else if (stateTimer <= 0f)
        {
            // Close enough to the player? Then this hop is a fast lunge straight into them
            bool lunge = chasing && (player.position - transform.position).sqrMagnitude <= lungeRange * lungeRange;

            bool hasPath = chasing ? RepathChase() : PickWanderPath();

            if (hasPath)
            {
                isHopping = true;
                isLunging = lunge;
                MONSTER_SPEED = lunge ? baseSpeed * lungeSpeedMultiplier : baseSpeed;
                currentHopDuration = lunge ? lungeDuration : hopDuration;
                stateTimer = currentHopDuration;
            }
            else
            {
                stateTimer = pauseDuration; // nowhere to go, try again after a rest
            }
        }
    }

    private void EndHop(float restTime)
    {
        isHopping = false;
        isLunging = false;
        MONSTER_SPEED = baseSpeed;
        stateTimer = restTime;
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

        // Path all the way to the player's tile so the slime actually jumps into them
        var newPath = PathfindingUtils.FindPath(start, target, walkable);
        if (newPath == null || newPath.Count == 0) return false;

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

    // Damage on collision. OnCollisionStay2D is used (not Enter) so this doesn't
    // shadow Monster's private OnCollisionEnter2D, which handles player attacks hitting the slime.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("player")) return;
        if (Time.time < nextContactDamageTime) return;

        HealthScript.RemoveHealth(MONSTER_DAMAGE);
        nextContactDamageTime = Time.time + contactCooldown;

        // Splat: stop the current hop and recover before the next one
        EndHop(lungeRecovery);
        path = null;
    }

    protected override void animateMonster()
    {
        // 0 at launch, 1 at landing
        float progress = isHopping ? 1f - Mathf.Clamp01(stateTimer / currentHopDuration) : 0f;

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
        // Unused: the slime damages the player by colliding with them (see OnCollisionStay2D).
    }

    protected override void OnDrawGizmosSelectedExtra()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lungeRange);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}