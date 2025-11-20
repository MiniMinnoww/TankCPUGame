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
        private float rotationVelocity = 0;

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

        protected override void OnFixedUpdate()
        {
            // Move
            Owner.Rb.linearVelocity = Brain.DriveInput * movementStats.moveSpeed * Owner.transform.up;

            // Turn
            float extraRotation = Mathf.Abs(Brain.DriveInput) * movementStats.extraRotationWhenMoving;
            float deltaRotation = (movementStats.rotationSpeed + extraRotation) * Brain.RotationInput * Time.fixedDeltaTime;

            if (deltaRotation != 0)
                rotationVelocity = deltaRotation;
            else
                rotationVelocity *= 0.9f * Time.fixedDeltaTime;

            Owner.Rb.MoveRotation(Owner.Rb.rotation - rotationVelocity);
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