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

    public void paintFloorTiles(IEnumerable<Vector2Int> floorPos, List<FloorEntry> floorTiles)
    {
        foreach (var pos in floorPos)
        {
            paintFloor(pos, floorTiles);
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
        ClearAllMonsters();
    }

    internal void paintWall(Vector2Int pos, TileBase wallTile)
    {
        PaintSingleTile(wallMap, wallTile, pos);
    }

    internal void paintFloor(Vector2Int pos, List<FloorEntry> floorTiles)
    {
        PaintSingleTile(floorTilemap, GetRandomFloorTile(floorTiles), pos);
    }

    public void ClearAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("object");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    public void ClearAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            Destroy(monster);
        }
    }

    private TileBase GetRandomFloorTile(List<FloorEntry> floorEntries)
    {
        float totalWeight = 0f;
        foreach (var entry in floorEntries)
        {
            totalWeight += entry.spawnChance;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (var entry in floorEntries)
        {
            cumulativeWeight += entry.spawnChance;
            if (randomValue <= cumulativeWeight)
            {
                return entry.floorTile;
            }
        }
        return null;
    }
}