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
        private float rotationVelocity;
        private Vector2 movementVelocity;
        private const float DECELERATION_RATE = 5f;

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
            if (Mathf.Abs(Brain.DriveInput) > 0.1f)
                movementVelocity = Brain.DriveInput * movementStats.moveSpeed * Owner.transform.up;

            Owner.Rb.linearVelocity = movementVelocity;
            

            // Turn
            float extraRotation = Mathf.Abs(Brain.DriveInput) * movementStats.extraRotationWhenMoving;
            float deltaRotation = (movementStats.rotationSpeed + extraRotation) * Brain.RotationInput * Time.fixedDeltaTime;

            if (deltaRotation != 0)
                rotationVelocity = deltaRotation;

            Owner.Rb.MoveRotation(Owner.Rb.rotation - rotationVelocity);
            
            // Lerp the velocities
            movementVelocity = Vector2.Lerp(movementVelocity, Vector2.zero, DECELERATION_RATE * Time.fixedDeltaTime);
            rotationVelocity = Mathf.Lerp(rotationVelocity, 0, DECELERATION_RATE * Time.fixedDeltaTime);

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