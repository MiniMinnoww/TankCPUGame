using Brains;
using Players;

namespace States.PlayerStates
{
    public class HitstunState : BaseState
    {
        public HitstunState(Player owner, Brain brain, float hitstun) : base(owner, brain, 0, hitstun) {}

        protected override void AfterStartLag() => StartEndLag();
        protected override void AfterEndLag() => Owner.SwitchState("movement");
    }
}