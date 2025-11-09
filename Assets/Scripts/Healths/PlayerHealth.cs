using UnityEngine.UI;

namespace Healths
{
    public class PlayerHealth : Health
    {
        private readonly Slider healthSlider;

        public PlayerHealth(float _maxHealth, Slider slider) : base(_maxHealth)
        {
            healthSlider = slider;

            UpdateHealthUI();
        }

        protected sealed override void UpdateHealthUI()
        {
            if (healthSlider == null) return;
            healthSlider.maxValue = MaxHealth;
            healthSlider.value = CurrentHealth;
        }
    }
}