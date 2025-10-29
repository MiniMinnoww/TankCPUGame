using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    private List<Hurtbox> hitPlayers = new();
    private Hurtbox owner;
    private float ttl;

    protected Hurtbox Owner => owner;

    public Projectile Fire(Vector2 direction, float ttl, Hurtbox owner)
    {
        hitPlayers.Clear();
        this.owner = owner;

        this.ttl = ttl;

        OnFire(direction);
        return this;
    }
    public abstract void OnFire(Vector2 direction);

    public abstract bool TryHitTarget(Hurtbox hurtbox);

    private void Update()
    {
        ttl -= Time.deltaTime;
        if (ttl <= 0) Kill(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Hurtbox hurtbox))
        {
            if (hurtbox == owner) return;
            if (hitPlayers.Contains(hurtbox)) return; // Only hit a player once

            if (TryHitTarget(hurtbox))
                hitPlayers.Add(hurtbox);
        }
    }

    protected abstract void Kill(bool deathAnimation = true);
}
