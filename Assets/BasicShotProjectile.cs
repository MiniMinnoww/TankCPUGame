using UnityEngine;
using System;

public class BasicShotProjectile : PhysicsProjectile
{
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private Transform sprite;

    public override void OnFire(Vector2 direction)
    {
        direction = direction.normalized;
        base.OnFire(direction);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sprite.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override bool TryHitTarget(Hurtbox hurtbox)
    {
        hurtbox.Damage(data.damage);

        if (data.destroyOnHit)
        {
            Kill();
        }

        return true;
    }

    protected override void Kill(bool deathAnimation=true)
    {
        if (deathAnimation)
        {
            deathParticles.transform.parent = null;
            deathParticles.Play();
        }
        Destroy(gameObject);
    }
}
