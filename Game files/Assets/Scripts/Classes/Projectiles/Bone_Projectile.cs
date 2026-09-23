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
        transform.Rotate(Vector3.forward * 30f * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.collider.CompareTag("player"))
        {
            HealthScript.RemoveHealth(PROJECTILE_DAMAGE);
            return;
        }

        Monster monster = collision.collider.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(PROJECTILE_DAMAGE);
            return;
        }

        WorldObject worldObject = collision.collider.GetComponent<WorldObject>();
        if (worldObject != null)
        {
            worldObject.Damage(0.1f, PROJECTILE_DAMAGE);
        }
    }
}