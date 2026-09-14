//This script was initially made using content shown in this video:https://www.youtube.com/watch?v=W6cBwk0bRWE&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=6
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TilemapDrawer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallMap;

    [SerializeField]
    private TileBase floorTile, topWall;

    public void paintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        paintTiles(floorPos, floorTilemap, floorTile);
    }

    private void paintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }


    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallMap.ClearAllTiles();
        ClearAllObjects();
    }

    internal void paintWall(Vector2Int pos)
    {
        PaintSingleTile(wallMap, topWall, pos);
    }

    public void ClearAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("object");

        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
}
