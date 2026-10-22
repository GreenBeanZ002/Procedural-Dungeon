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
        PROJECTILE_COLLISION_DELAY = 0f; // owner collider is ignored instead, so no delay needed
    }

    protected override void animateProjectile()
    {
        transform.Rotate(Vector3.forward * 30f * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        // NOTE: tags are case-sensitive. Make sure your Player is tagged exactly "player".
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