using Brains;
using Hurtboxes;
using Players;
using UnityEngine;

namespace States.PlayerStates
{
    // NOTE: Probably not implemented in the end, out of scope
    public class BlockingState : BaseState
    {
        private TankMovementStats movementStats;
        private readonly PlayerHurtbox playerHurtbox;

        private float timeInMove = 0;

        public BlockingState(Player owner, Brain brain, PlayerHurtbox hurtbox) : base(owner, brain)
        {
            playerHurtbox = hurtbox;
            //playerHurtbox.isBlocking = true;
        }

        protected override void OnUpdate()
        {
            timeInMove += Time.deltaTime;
        }
    }
}