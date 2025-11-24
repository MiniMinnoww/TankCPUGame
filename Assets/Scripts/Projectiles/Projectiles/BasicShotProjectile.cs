using UnityEngine;
using Hurtboxes;
using Managers;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Projectiles.Projectiles
{
    public class BasicShotProjectile : PhysicsProjectile
    {
        [SerializeField] private ParticleSystem deathParticles;
        [FormerlySerializedAs("sprite")] [SerializeField] private Transform spriteParent;
        [SerializeField] private SpriteRenderer projectileRenderer;
        [SerializeField] private Light2D explosionLight;
        [SerializeField] private TrailRenderer trail;
        private Vector2 dir;

        public override void SetupColour(int colourIndex)
        {
            projectileRenderer.sprite = Global.Sprites[colourIndex].bulletSprite;

            Color startColour = Global.Sprites[colourIndex].colour;
            
            Color endColor = startColour;
            endColor.a = 0;

            trail.startColor = startColour;
            trail.endColor = endColor;
        }
        public override void OnFire(Vector2 direction)
        {
            direction = direction.normalized;
            dir = direction;
            base.OnFire(direction);

            explosionLight.enabled = false;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spriteParent.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected override bool TryHitTarget(Hurtbox hurtbox)
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

        public override Vector2 GetDirection() => Rb.linearVelocity.normalized;
    }
}

