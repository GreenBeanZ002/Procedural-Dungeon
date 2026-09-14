//This script was initially made using content shown in this video:https://www.youtube.com/watch?v=U3Wr-sNnJNk&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonGenerator : MonoBehaviour
{
    [SerializeField] 
    protected TilemapDrawer tilemapDraw= null;
    [SerializeField] 
    protected Vector2Int startPos = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapDraw.Clear();
        RunProceduralGeneration();
    }
    protected abstract void RunProceduralGeneration();
}
