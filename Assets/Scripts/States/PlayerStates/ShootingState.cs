using Brains;
using Hurtboxes;
using Players;
using Projectiles;
using UnityEngine;
using VFX;

namespace States.PlayerStates
{
    public class ShootingState : BaseState
    {
        private readonly Projectile projectilePrefab;
        private readonly Hurtbox playerHurtbox;
        public ShootingState(Player owner, Brain brain, Hurtbox hurtbox, Projectile projectilePrefab, float startLag, float endLag) : base(owner, brain, startLag, endLag)
        {
            this.projectilePrefab = projectilePrefab;
            playerHurtbox = hurtbox;
        }

        protected override void AfterStartLag()
        {
            Projectile projectile = Object.Instantiate(projectilePrefab, Owner.transform.position, Quaternion.identity);
            projectile.SetupColour(Owner.ColourIndex);
            projectile.Fire(Owner.transform.up, 5, playerHurtbox);
        
            ScreenShake.ShakeScreen(ScreenShake.smallShake);

            StartEndLag();
        }

        protected override void AfterEndLag() => Owner.SwitchState("movement");
    }
}