using UnityEngine;
using System;

public abstract class PhysicsProjectile : Projectile
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected ShotProjectileData data;

    protected Rigidbody2D RB => rb;

    public override void OnFire(Vector2 direction)
    {
        rb.AddForce(direction * data.speed, ForceMode2D.Impulse);
    }
}
