using UnityEngine;

public class HitstunState : BaseState
{
    public HitstunState(Player player, Brain brain, float hitstun) : base(player, brain, 0, hitstun) {}

    protected override void AfterStartLag() => StartEndLag();
    protected override void AfterEndLag() => Player.SwitchState("movement");
}