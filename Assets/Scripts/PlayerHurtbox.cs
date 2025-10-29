public class PlayerHurtbox : Hurtbox
{
    // TODO: Once shield is implemented will need to change
    public override void Damage(float amount) => Health.Damage(amount);
}