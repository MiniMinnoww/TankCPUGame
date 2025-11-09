using UnityEngine;
using Healths;

namespace Hurtboxes
{
    public delegate void OnHurtboxHit(Hurtbox h);
    public abstract class Hurtbox : MonoBehaviour
    {
        protected Health Health { get; private set; }
        public event OnHurtboxHit OnHurtboxHitEvent;

        public virtual void Setup(Health h)
        {
            Health = h;
        }
        public virtual void Damage(float amount, Vector2 knockbackDirection, Vector2 hitPoint)
        {
            Health.Damage(amount);
            OnHurtboxHitEvent?.Invoke(this);
        } 
        public virtual void Heal(float amount) => Health.Heal(amount);
    }

}
