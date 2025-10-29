using UnityEngine;

public class ShootingState : BaseState
{
    private Projectile projectilePrefab;
    private Hurtbox playerHurtbox;
    public ShootingState(Player player, Brain brain, Hurtbox hurtbox, Projectile projectilePrefab, float startLag, float endLag) : base(player, brain, startLag, endLag)
    {
        this.projectilePrefab = projectilePrefab;
        playerHurtbox = hurtbox;
    }

    protected override void AfterStartLag()
    {
        Projectile projectile = Player.Instantiate(projectilePrefab, Player.transform.position, Quaternion.identity);
        projectile.Fire(Player.transform.up, 5, playerHurtbox);

        StartEndLag();
    }

    protected override void AfterEndLag() => Player.SwitchState("movement");
}