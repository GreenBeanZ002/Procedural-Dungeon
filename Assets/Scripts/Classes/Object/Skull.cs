using UnityEngine;

public class Skull : WorldObject
{
    [SerializeField]
    private Sprite skullSprite;

    protected override void setDefaults()
    {
        OBJECT_NAME = "Skull";
        HEALTH = 2;
        ICON = skullSprite;
    }
}
