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


    

    private SpriteRenderer spriteRenderer;



    protected virtual void Awake()
    {
        setDefaults();
        spriteRenderer = GetComponent<SpriteRenderer>();
        setupIcon();
    }

    protected void setupIcon()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = PROJECTILE_ICON;
        }
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
        transform.position = new Vector3(transform.position.x + movement.x,transform.position.y + movement.y,-0.2f);
    }
}