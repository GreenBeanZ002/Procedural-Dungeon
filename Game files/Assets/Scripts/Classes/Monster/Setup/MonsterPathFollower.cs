using UnityEngine;
using System.Collections.Generic;

public class MonsterPathFollower:MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    public float repathInterval;
    public float tileSize = 1f;

    private List<Vector2Int> path;
    private int pathIndex;
    private float repathTimer;
    private PathfindingUtils pathfindingUtils;
    private bool isChasing = false;

    [Header("Detection")]
    public float detectionRange = 5f;
    public float loseRange = 8f;

    void Start() 
    {
        pathfindingUtils = GetComponent<PathfindingUtils>();
    }

    void Update() 
    {
        float sqrDist = (player.position - transform.position).sqrMagnitude;

        if (!isChasing && sqrDist <= detectionRange * detectionRange)
        {
            isChasing = true;
            repathTimer = 0f; 
        }
        else if (isChasing && sqrDist > loseRange * loseRange)
        {
            isChasing = false;
            path = null; 
        }
        if (!isChasing) { return; }

        repathTimer -= Time.deltaTime;
        if (repathTimer < 0f) 
        {
            Repath();
            repathTimer = repathInterval;
        }

        FollowPath();
    }
    void Repath()
    {
        var walkableTiles = pathfindingUtils.GetWalkableTiles();
        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int target = WorldToGrid(player.position);
        path = PathfindingUtils.FindPath(start, target, walkableTiles);
        pathIndex = 0;
    }

    void FollowPath() 
    { 
        if (path == null || pathIndex >= path.Count) { return; }

        Vector3 TargetPos = GridToWorld(path[pathIndex]);
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, moveSpeed* Time.deltaTime);

        if(Vector3.Distance(transform.position, TargetPos) < 0.1f)
        {
            pathIndex++;
        }
    }

    Vector2Int WorldToGrid(Vector3 worldPos){    return new Vector2Int(Mathf.RoundToInt(worldPos.x / tileSize), Mathf.RoundToInt(worldPos.y / tileSize));}

    private Vector3 GridToWorld(Vector2Int gridPos) =>
    new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, transform.position.z);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }



}