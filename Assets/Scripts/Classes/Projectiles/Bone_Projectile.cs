using UnityEngine;

public class Bone : Projectile
{
    protected override void setDefaults()
    {
        PROJECTILE_NAME = "Bone";
        PROJECTILE_ICON = Resources.Load<Sprite>("Sprites/Projectiles/Bone");
        PROJECTILE_DIRECTION = Vector2.right;
        PROJECTILE_SPEED = 3f;
        PROJECTILE_DAMAGE = 10;
    }

    protected override void animateProjectile()
    {
        transform.Rotate(Vector3.forward * 3f * Time.deltaTime);
    }

    
}
