//This script was initially made using content shown in this video: https://www.youtube.com/watch?v=F_Zc1nvtB0o&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=4
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGeneration
{

    public static HashSet<Vector2Int> RandomPath(Vector2Int startPos, int walkLen)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var previousPos = startPos;

        for (int i = 0; i < walkLen; i++) 
        {
            var newPos = previousPos + direction2D.getRandomDirection();
            path.Add( newPos );
            previousPos = newPos;
        }
        return path;
    }
}

public static class direction2D
{
    public static List<Vector2Int> directionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), //UP
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(1, 0), //RIGHT
        new Vector2Int(-1, 0) //LEFT
    };
    public static Vector2Int getRandomDirection()
    {
        return directionsList[Random.Range(0, directionsList.Count)];
    }
}
