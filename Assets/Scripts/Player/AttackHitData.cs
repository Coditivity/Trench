using UnityEngine;

public struct AttackHitData
{
    public RaycastHit raycastHit;
    public int damage;
    /// <summary>
    /// For weapons that fire multiple projectiles per shot, this will be true for the last projectile
    /// </summary>
    public bool bLastProjectile;
    public AttackHitData(RaycastHit raycastHit, int damage, bool bLastProjectile)
    {
        this.raycastHit = raycastHit;
        this.damage = damage;
        this.bLastProjectile = bLastProjectile;
    }
}
