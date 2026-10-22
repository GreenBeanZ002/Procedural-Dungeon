using System.Collections.Generic;

public class SewerDungeon : DungeonArea
{
    protected override void SetDefaults()
    {
        AREA_NAME = "Sewer";
        START_LEVEL = "11";
        END_LEVEL = "20";
        BOSS_LEVEL = 20;
        AREA_DEFAULT = false;


        floorTilesToSpawn ??= new List<FloorEntry>();
        wallTilesToSpawn ??= new List<WallEntry>();
        objectsToSpawn ??= new List<ObjectEntry>();
        monstersToSpawn ??= new List<MonsterEntry>();
    }
}