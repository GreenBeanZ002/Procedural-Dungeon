using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CryptDungeon : DungeonArea
{
    protected override void SetDefaults()
    {
        AREA_NAME = "Crypt";
        LEVEL_COUNT = "10";
        BOSS_LEVEL = 10;

        if (floorTilesToSpawn == null || floorTilesToSpawn.Count == 0)
        {
            floorTilesToSpawn = new List<FloorEntry>
            {
                new FloorEntry { floorTile = Resources.Load<TileBase>("Tiles/Crypt/CryptFloor"), spawnChance = 100f }
            };
        }

        if (wallTilesToSpawn == null || wallTilesToSpawn.Count == 0)
        {
            wallTilesToSpawn = new List<WallEntry>
            {
                new WallEntry { wallTile = Resources.Load<TileBase>("Tiles/Crypt/CryptWall"), spawnChance = 80f },
                new WallEntry { wallTile = Resources.Load<TileBase>("Tiles/Crypt/CryptWallCracked"), spawnChance = 20f }
            };
        }

        objectsToSpawn ??= new List<ObjectEntry>();
        monstersToSpawn ??= new List<MonsterEntry>();
    }
}