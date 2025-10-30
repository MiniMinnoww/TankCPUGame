using UnityEngine;

public class MovementState : BaseState
{
    private TankMovementStats movementStats;
    private PlayerHurtbox playerHurtbox;

    public MovementState(Player player, Brain brain, PlayerHurtbox hurtbox, TankMovementStats stats) : base(player, brain, 0, 0)
    {
        movementStats = stats;
        playerHurtbox = hurtbox;
    }

    protected override void RegisterEvents()
    {
        Brain.OnShootEvent += OnShoot;
        playerHurtbox.OnHurtboxHitAndDamagedEvent += OnHit;
    }

    protected override void UnregisterEvents()
    {
        Brain.OnShootEvent -= OnShoot;
        playerHurtbox.OnHurtboxHitAndDamagedEvent -= OnHit;
    }

    protected override void OnUpdate()
    {
        // Move
        Player.RB.linearVelocity = Brain.DriveInput * movementStats.moveSpeed * Player.transform.up;

        // Turn
        float extraRotation = Mathf.Abs(Brain.DriveInput) * movementStats.extraRotationWhenMoving;
        float deltaRotation = (movementStats.rotationSpeed + extraRotation) * Brain.RotationInput * Time.fixedDeltaTime;

        Player.RB.MoveRotation(Player.RB.rotation - deltaRotation);
    }

    private void OnShoot()
    {
        Player.SwitchState("shooting");
    }

    private void OnHit(Hurtbox h, float amount, Vector2 knockback)
    {
        Player.SwitchState("hitstun");
    }
}