
using UnityEngine;

public delegate void OnHit(Health health);
public delegate void OnHeal(Health health);
public delegate void OnKill(Health health);

public abstract class Health
{
    private float health;
    private float maxHealth;

    protected float CurrentHealth { get => health; private set => health = Mathf.Clamp(value, 0, MaxHealth); }
    protected float MaxHealth { get => maxHealth; set => maxHealth = Mathf.Clamp(value, 0, Mathf.Infinity); }

    public event OnHit OnHitEvent;
    public event OnHeal OnHealEvent;
    public event OnKill OnKillEvent;

    public Health(float _maxHealth)
    {
        MaxHealth = _maxHealth;
        CurrentHealth = MaxHealth;
    }

    public virtual void Damage(float amount) 
    {
        CurrentHealth -= amount;
        OnHitEvent?.Invoke(this);

        UpdateHealthUI();
        CheckForKill();
    }

    public virtual void Heal(float amount) 
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
        OnKillEvent?.Invoke(this);
    }
    public virtual void UpdateHealthUI() {}
}