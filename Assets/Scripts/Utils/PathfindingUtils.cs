using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUtils : MonoBehaviour
{
    // Start is called before the first frame update
    public HashSet<Vector2Int> GetWalkableTiles()
    {
        // Implementation for pathfinding
        HashSet<Vector2Int> floorPos = RandomWalkMapGenerator.getFloorPositions();
        HashSet<Vector2Int> objectPos = ObjectUtils.getObjectPositions(); // Assuming ObjectUtils is a class that provides object positions
        foreach (Vector2Int obj in objectPos)
        {
            if (floorPos.Contains(obj))
            {
                floorPos.Remove(obj);
            }
        }
        return floorPos;
    }
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int target, HashSet<Vector2Int> walkableTiles)
    {
        var openSet = new List<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, int> { [start] = Heuristic(start, target) };

        Vector2Int[] directions = {
            Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right
        };
        {
            while (openSet.Count > 0)
            {
                Vector2Int current = openSet[0];
                foreach (var node in openSet)
                {
                    if (fScore.GetValueOrDefault(node, int.MaxValue) < fScore.GetValueOrDefault(current, int.MaxValue))
                    {
                        current = node;
                    }                      
                    
                }
                if (current == target) 
                { 
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                foreach(var direction in directions)
                {
                    Vector2Int neighbour = current + direction;
                    if(!walkableTiles.Contains(neighbour))
                    {
                        continue;
                    }
                    
                    int tentativeG = gScore[current] + 1;
                    if(tentativeG < gScore.GetValueOrDefault(neighbour, int.MaxValue))
                    {
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = tentativeG;
                        fScore[neighbour] = tentativeG + Heuristic(neighbour, target);
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }

                }
            }
            return new List<Vector2Int>();
        }

        static int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }
}
