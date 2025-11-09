using UnityEngine;
using VFX;

namespace Hurtboxes
{
    public delegate void OnHurtboxHitAndDamaged(Hurtbox h, float damage, Vector2 knockbackDirection);
    public class PlayerHurtbox : Hurtbox
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float knockbackOnHit;
        public event OnHurtboxHitAndDamaged OnHurtboxHitAndDamagedEvent;
        // TODO: Once shield is implemented will need to change
        public override void Damage(float amount, Vector2 knockbackDirection, Vector2 hitPoint)
        {
            Health.Damage(amount);
            rb.AddForce(knockbackDirection * knockbackOnHit, ForceMode2D.Impulse);
        
            ScreenShake.ShakeScreen(ScreenShake.mediumShake);

            OnHurtboxHitAndDamagedEvent?.Invoke(this, amount, knockbackDirection);
        }
    }
}