using Brains;
using Hurtboxes;
using Players;
using UnityEngine;

namespace States.PlayerStates
{
    public class MovementState : BaseState
    {
        private readonly TankMovementStats movementStats;
        private readonly PlayerHurtbox playerHurtbox;

        public MovementState(Player owner, Brain brain, PlayerHurtbox hurtbox, TankMovementStats stats) : base(owner, brain)
        {
            movementStats = stats;
            playerHurtbox = hurtbox;
        }

        protected override void RegisterEvents()
        {
            Brain.onShootEvent += OnShoot;
            playerHurtbox.OnHurtboxHitAndDamagedEvent += OnHit;
        }

        protected override void UnregisterEvents()
        {
            Brain.onShootEvent -= OnShoot;
            playerHurtbox.OnHurtboxHitAndDamagedEvent -= OnHit;
        }

        protected override void OnUpdate()
        {
            // Move
            Owner.Rb.linearVelocity = Brain.DriveInput * movementStats.moveSpeed * Owner.transform.up;

            // Turn
            float extraRotation = Mathf.Abs(Brain.DriveInput) * movementStats.extraRotationWhenMoving;
            float deltaRotation = (movementStats.rotationSpeed + extraRotation) * Brain.RotationInput * Time.fixedDeltaTime;

            Owner.Rb.MoveRotation(Owner.Rb.rotation - deltaRotation);
        }

        private void OnShoot()
        {
            Owner.SwitchState("shooting");
        }

        private void OnHit(Hurtbox h, float amount, Vector2 knockback)
        {
            Owner.SwitchState("hitstun");
        }
    }
}