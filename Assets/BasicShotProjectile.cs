using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class BasicShotProjectile : PhysicsProjectile
{
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private Transform sprite;
    [SerializeField] private Light2D explosionLight;
    private Vector2 dir;

    public override void OnFire(Vector2 direction)
    {
        direction = direction.normalized;
        dir = direction;
        base.OnFire(direction);

        explosionLight.enabled = false;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sprite.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override bool TryHitTarget(Hurtbox hurtbox)
    {
        hurtbox.Damage(data.damage, dir, transform.position);

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
            explosionLight.enabled = true;
            deathParticles.Play();
        }
        Destroy(gameObject);
    }
}
