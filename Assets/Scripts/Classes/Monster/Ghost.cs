using UnityEngine;

public class Ghost : Monster 
{
    [SerializeField] private Sprite monsterSprite;

    protected override void SetDefaults()
    {
        MONSTER_NAME = "Ghost";
        MONSTER_HEALTH = 5;
        MONSTER_SPEED = 7f;
        MONSTER_DAMAGE = 0;
        MONSTER_SPRITE = monsterSprite;
    }
}

