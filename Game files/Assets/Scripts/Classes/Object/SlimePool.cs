using UnityEngine;

public class  SlimePool : WorldObject
{
    [SerializeField]
    private Sprite poolSprite;

    protected override void setDefaults()
    {
        OBJECT_NAME = "Slime Pool";
        HEALTH = 2;
        ICON = poolSprite;
    }
    protected override void DeathEvent()
    {
        return;
    }

}