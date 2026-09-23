//This script was initially made using content shown in this video:https://www.youtube.com/watch?v=LdpItlRg8OM&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=9
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallDrawer 
{
    public static void DrawWalls(HashSet<Vector2Int> floorPos, TilemapDrawer tilemapDraw)
    {
        var wallPos = FindWalls(floorPos, direction2D.directionsList);
        foreach (var pos in wallPos)
        {
            tilemapDraw.paintWall(pos);
        }
    }

    private static HashSet<Vector2Int> FindWalls(HashSet<Vector2Int> floorPos, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach(var pos in floorPos)
        {
            foreach(var direction in directionList)
            {
                var neighbour = pos + direction;
                if (floorPos.Contains(neighbour) == false){
                    wallPos.Add(neighbour);
                }
                {
                    
                }
            }
        }
        return wallPos;
    }
}
