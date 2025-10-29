using UnityEngine;

public abstract class Hurtbox : MonoBehaviour
{
    protected Health Health { get; private set; }

    public void Setup(Health h)
    {
        Health = h;
    }
    public virtual void Damage(float amount) => Health.Damage(amount);
    public virtual void Heal(float amount) => Health.Heal(amount);
}
