using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CryptDungeon : DungeonArea
{
    protected override void SetDefaults()
    {
        AREA_NAME = "Crypt";
        START_LEVEL = "21";
        END_LEVEL = "30";
        BOSS_LEVEL = 30;
        AREA_DEFAULT = true;


        floorTilesToSpawn ??= new List<FloorEntry>();
        wallTilesToSpawn ??= new List<WallEntry>();
        objectsToSpawn ??= new List<ObjectEntry>();
        monstersToSpawn ??= new List<MonsterEntry>();
    }
}
