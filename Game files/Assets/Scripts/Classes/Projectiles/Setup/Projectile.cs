using System;
using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected string PROJECTILE_NAME;

    public string ProjectileName => PROJECTILE_NAME;

    [SerializeField]
    protected Sprite PROJECTILE_ICON;

    public Sprite ProjectileIcon => PROJECTILE_ICON;

    [SerializeField]
    protected Vector2 PROJECTILE_DIRECTION;

    public Vector2 ProjectileDirection => PROJECTILE_DIRECTION;

    [SerializeField]
    protected float PROJECTILE_SPEED;

    public float ProjectileSpeed => PROJECTILE_SPEED;

    [SerializeField]
    protected int PROJECTILE_DAMAGE;

    public int ProjectileDamage => PROJECTILE_DAMAGE;


    [SerializeField]
    protected float PROJECTILE_LIFETIME = 5f;

    public float ProjectileLifetime => PROJECTILE_LIFETIME;

    [SerializeField]
    protected bool PROJECTILE_USE_LIFETIME = true;

    public bool ProjectileUseLifetime => PROJECTILE_USE_LIFETIME;

    [SerializeField]
    protected float PROJECTILE_COLLISION_DELAY = 0.44f;

    public float ProjectileCollisionDelay => PROJECTILE_COLLISION_DELAY;


    private SpriteRenderer spriteRenderer;
    private Collider2D projectileCollider;





    protected virtual void Awake()
    {
        setDefaults();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<Collider2D>();
        SetupSprite();

        if (PROJECTILE_USE_LIFETIME)
        {
            Destroy(gameObject, PROJECTILE_LIFETIME);
        }

        if (PROJECTILE_COLLISION_DELAY > 0f && projectileCollider != null)
        {
            projectileCollider.enabled = false;
            StartCoroutine(EnableCollisionAfterDelay());
        }
    }

    private IEnumerator EnableCollisionAfterDelay()
    {
        yield return new WaitForSeconds(PROJECTILE_COLLISION_DELAY);
        projectileCollider.enabled = true;
    }


    protected abstract void setDefaults();

    public void SetDamage(int amount)
    {
        PROJECTILE_DAMAGE = amount;

    }

    protected virtual void Update()
    {
        animateProjectile();
        moveProjectile(ProjectileDirection, ProjectileSpeed);
    }


    protected abstract void animateProjectile();

    private void moveProjectile(Vector2 projectileDirection, float moveSpeed)
    {
        Vector2 movement = projectileDirection.normalized * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + movement.x, transform.position.y + movement.y, -0.2f);
    }

    public virtual void summonProjectile(GameObject prefab, int count, Vector3 spawnPosition, Vector3 target, Sprite projectileSprite)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Projectile spawnedScript = spawned.GetComponent<Projectile>();
            spawnedScript.PROJECTILE_DIRECTION = getDirection(spawnPosition, target);
            spawnedScript.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        }
    }

    protected void SetupSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = PROJECTILE_ICON;
        }
    }
    private Vector3 getDirection(Vector3 start, Vector3 target)
    {
        Vector3 direction = (target - start).normalized;
        return direction;
    }
}