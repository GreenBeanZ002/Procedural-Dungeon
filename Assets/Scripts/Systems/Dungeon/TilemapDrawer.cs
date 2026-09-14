//This script was initially made using content shown in this video:https://www.youtube.com/watch?v=W6cBwk0bRWE&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=6
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        var positionList = positions.ToList();

        foreach (var pos in positionList)
        {
            PaintSingleTile(tilemap, tile, pos);
        }

/*        Vector3 spawnPos = (Vector3Int)positionList[UnityEngine.Random.Range(0, positionList.Count)];
        spawnPos.z = -0.4f; // Set the z position to -0.4f
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPos;*/
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
