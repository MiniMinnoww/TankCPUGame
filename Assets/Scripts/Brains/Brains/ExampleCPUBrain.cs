using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Brains.Brains
{
    [Serializable]
    public class ExampleCPUBrain : BaseCPUBrain
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public ExampleCPUBrain() {}
        public ExampleCPUBrain(IPlayerBrainInterface player) : base(player) {}
        public override void Update()
        {
            // If the closest projectile isn't ours
            if (IsEnemyProjectileInView())
            {
                    // Move backwards to try and avoid it
                    MoveForSeconds(-1, 1);
                    return;
            }
            
            // If there is an enemy in our view cone
            IDetectableTank closestTank = GetClosestTank();
            if (closestTank != null)
            {
                float targetRotation = GetRotationToLookAt(closestTank);
                SetRotationInput(targetRotation);
                StopDriving();
                
                if (IsTankShootableAhead()) Shoot();
            }
            else
            {
                SetRotationInput(Mathf.Sin(Time.time));
                
                // Just move around
                if (IsObstacleAhead(range: 5))
                    SetDriveInput(-1);
                else
                    SetDriveInput(1);
                    
            }
        }
    }
}