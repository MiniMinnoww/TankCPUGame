using System.Collections.Generic;
using Brains;
using Hurtboxes;
using NUnit.Framework.Constraints;
using Players;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile : MonoBehaviour, IDetectableProjectile
    {
        private readonly List<Hurtbox> hitPlayers = new();
        private float ttl;

        private Hurtbox Owner { get; set; }

        public Projectile Fire(Vector2 direction, float timeToLive, Hurtbox owner)
        {
            hitPlayers.Clear();
            Owner = owner;
            ttl = timeToLive;

            OnFire(direction);
            return this;
        }

        public abstract void SetupColour(int colourIndex);

        public abstract void OnFire(Vector2 direction);

        protected abstract bool TryHitTarget(Hurtbox hurtbox);

        private void Update()
        {
            ttl -= Time.deltaTime;
            if (ttl <= 0) Kill(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out Hurtbox hurtbox)) return;
            if (hurtbox == Owner) return;
            if (hitPlayers.Contains(hurtbox)) return; // Only hit a player once

            if (TryHitTarget(hurtbox))
                hitPlayers.Add(hurtbox);
        }

        protected abstract void Kill(bool deathAnimation = true);
        
        public ObjectType GetObjectType() => ObjectType.Projectile;
        public Vector2 GetPosition() => transform.position;

        public IDetectableTank GetOwner()
        {
            return Owner ? Owner.GetComponent<Player>() : null;
        }

        public abstract Vector2 GetDirection();
    }
}
