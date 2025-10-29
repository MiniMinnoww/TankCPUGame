using UnityEngine;

public class MovementState : BaseState
{
    private TankMovementStats movementStats;

    public MovementState(Player player, Brain brain, TankMovementStats stats) : base(player, brain, 0, 0)
    {
        movementStats = stats;
    }

    protected override void RegisterEvents()
    {
        Brain.onShoot += OnShoot;
    }

    protected override void UnregisterEvents()
    {
        Brain.onShoot -= OnShoot;
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
}