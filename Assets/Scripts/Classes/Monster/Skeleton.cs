using System.Collections.Generic;

using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] private Sprite monsterSprite;

    [Header("Flee Behavior")]
    [SerializeField] private bool canFlee = true;
    [SerializeField] private float fleeRange = 1.5f;
    [SerializeField] private float fleeDelay = 0.5f;
    [SerializeField] private float fleeExitRange = 3f;
    [SerializeField] private float fleeDistanceMin = 5f;
    [SerializeField] private float fleeDistanceMax = 15f;

    private float closeTimer = 0f;
    private bool isFleeing = false;
    private float currentFleeDistance;

    protected override void SetDefaults()
    {
        MONSTER_NAME = "Skeleton";
        MONSTER_HEALTH = 10;
        MONSTER_SPEED = 5f;
        MONSTER_DAMAGE = 10;
        MONSTER_SPRITE = monsterSprite;
    }

    protected override bool HandleCustomBehavior()
    {
        if (!canFlee) return false;

        float sqrDist = (player.position - transform.position).sqrMagnitude;

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

        if (!isFleeing) return false;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            RepathFlee();
            repathTimer = REPATH_INTERVAL;
        }

        FollowPath();
        return true;
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

        int fleeTiles = Mathf.Max(1, Mathf.RoundToInt(currentFleeDistance / TILE_SIZE));

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

    protected override void OnDrawGizmosSelectedExtra()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, fleeRange);
    }
}