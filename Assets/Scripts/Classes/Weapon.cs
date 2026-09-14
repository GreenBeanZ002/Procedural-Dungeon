using UnityEngine;


public enum DamageType
{
    Melee,
    Ranged,
    Magic
}

public abstract class Weapon : Item
{

    public abstract DamageType DamageType { get; }

    [SerializeField]
    protected int DAMAGE;

    public int Damage => DAMAGE;
}