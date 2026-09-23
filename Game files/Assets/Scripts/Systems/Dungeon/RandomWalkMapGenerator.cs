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

[System.Serializable]
public class MonsterEntry
{
    public GameObject monsterPrefab;
    [Range(0, 100)] public float spawnChance;
}
public class RandomWalkMapGenerator : DungeonGenerator
{
    public static HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> getFloorPositions()
    {
        return floorPos;
    }

    [SerializeField]
    private RandomWalkData walkParameters;

    [SerializeField]
    private List<ObjectEntry> objectsToSpawn; 

    [SerializeField]
    private List<MonsterEntry> monstersToSpawn; 

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
        MonsterUtils.DrawMonsters(floorPos, monstersToSpawn);
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
