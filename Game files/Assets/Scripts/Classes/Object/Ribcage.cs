using UnityEngine;

public class Ribcage : WorldObject
{
    [SerializeField]
    private Sprite ribcageSprite;

    protected override void setDefaults()
    {
        OBJECT_NAME = "Ribcage";
        HEALTH = 2;
        ICON = ribcageSprite;
    }
}