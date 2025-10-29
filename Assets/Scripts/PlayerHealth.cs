using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    private Slider healthSlider;

    public PlayerHealth(float _maxHealth, Slider slider) : base(_maxHealth)
    {
        healthSlider = slider;

        UpdateHealthUI();
    }

    public override void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHealth;
            healthSlider.value = CurrentHealth;
        }
    }
}