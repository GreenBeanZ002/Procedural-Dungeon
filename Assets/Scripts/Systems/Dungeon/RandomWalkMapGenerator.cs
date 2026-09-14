using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rand = UnityEngine.Random;

[System.Serializable]
public class ObjectEntry
{
    public WorldObject objectPrefab;
    [Range(0, 100)] public float spawnChance;
}
public class RandomWalkMapGenerator : DungeonGenerator
{
    public static HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> getFloorPositions()
    {
        Debug.Log($"getFloorPositions called, returning {floorPos.Count} tiles");
        return floorPos;
    }

    [SerializeField]
    private RandomWalkData walkParameters;

    [SerializeField]
    private List<ObjectEntry> objectsToSpawn; 

    [SerializeField]
    private GameObject monPrefab1; //redo like objects soon

    [SerializeField]
    private ObjectUtils objDraw;

    [SerializeField]
    private Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateDungeon();
        }
    }

    protected override void RunProceduralGeneration()
    {
        tilemapDraw.Clear();
        floorPos = RunRandomWalk();
        tilemapDraw.paintFloorTiles(floorPos);
        WallDrawer.DrawWalls(floorPos, tilemapDraw);
        objDraw.drawObjects(floorPos, player, objectsToSpawn);
        MonsterUtils.DrawMonsters(floorPos, monPrefab1);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPos = startPos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        for (int i = 0; i < walkParameters.iterations; i++)
        {
            var path = ProceduralGeneration.RandomPath(currentPos, walkParameters.pathLength);
            floorPos.UnionWith(path);

            if (walkParameters.randomStart)
            {
                currentPos = floorPos.ElementAt(Rand.Range(0, floorPos.Count));
            }
        }
        return floorPos;
    }
}
