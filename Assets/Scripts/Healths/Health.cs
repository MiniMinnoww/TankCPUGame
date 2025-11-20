
using UnityEngine;
using VFX;

namespace Healths
{
    public delegate void OnHit(Health health);

    public delegate void OnHeal(Health health);

    public delegate void OnKill(Health health);

    public abstract class Health
    {
        private float health;
        private float maxHealth;

        public float CurrentHealth
        {
            get => health;
            private set => health = Mathf.Clamp(value, 0, MaxHealth);
        }

        public float MaxHealth
        {
            get => maxHealth;
            private set => maxHealth = Mathf.Clamp(value, 0, Mathf.Infinity);
        }

        public event OnHit OnHitEvent;
        public event OnHeal OnHealEvent;
        public event OnKill OnKillEvent;

        protected Health(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
        }

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            OnHitEvent?.Invoke(this);

            UpdateHealthUI();
            CheckForKill();
        }

        public void Heal(float amount)
        {
            CurrentHealth -= amount;
            OnHealEvent?.Invoke(this);

            UpdateHealthUI();
        }

        private void CheckForKill()
        {
            if (CurrentHealth <= 0) Kill();
        }

        private void Kill()
        {
            ScreenShake.ShakeScreen(ScreenShake.largeShake);
            OnKillEvent?.Invoke(this);
        }

        protected virtual void UpdateHealthUI()
        {
        }
    }
}