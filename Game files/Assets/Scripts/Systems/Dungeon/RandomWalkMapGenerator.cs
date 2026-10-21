using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rand = UnityEngine.Random;

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
    private ObjectUtils objDraw;

    [SerializeField]
    private DungeonArea area;

    [SerializeField]
    private Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateDungeon(area);
        }
    }

    protected override void RunProceduralGeneration(List<WallEntry> walls, List<FloorEntry> floors, List<ObjectEntry> objects, List<MonsterEntry> monsters)
    {
        tilemapDraw.Clear();
        floorPos = RunRandomWalk();
        tilemapDraw.paintFloorTiles(floorPos, floors);
        WallDrawer.DrawWalls(floorPos, tilemapDraw, walls);
        objDraw.drawObjects(floorPos, player, objects);
        MonsterUtils.DrawMonsters(floorPos, monsters);

        if(player != null)
        {
            player.transform.position = new Vector3(0, 0, -0.3f);
        }
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
