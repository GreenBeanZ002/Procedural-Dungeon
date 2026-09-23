using UnityEngine;

public class Chest : WorldObject
{
    [SerializeField]
    private Sprite chestSprite;
    protected override void setDefaults()
    {
        OBJECT_NAME = "Chest";
        HEALTH = 3;
        ICON = chestSprite;
    }
}
