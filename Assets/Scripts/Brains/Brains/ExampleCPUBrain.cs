using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Brains.Brains
{
    [Serializable]
    public class ExampleCPUBrain : BaseCPUBrain
    {
        // ReSharper disable Unity.PerformanceAnalysis
        // ReSharper disable Unity.PerformanceCriticalCodeInvocation
        // ReSharper disable Unity.PerformanceCriticalCodeInvocation
        public ExampleCPUBrain() {}
        public ExampleCPUBrain(IPlayerBrainInterface player) : base(player) {}

        private float randomSeed;
        public override void Start()
        {
            // Get a random seed for this CPU
            randomSeed = Random.Range(0, 360);
        }

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
                SetRotationInput(Mathf.Sin(Time.time + randomSeed));

                if (Time.frameCount % 240 == 0) // Every 240 frames (~4s)
                    randomSeed = Random.Range(0, 360);
                
                // Just move around
                if (IsObstacleAhead(range: 5))
                    SetDriveInput(-1);
                else
                    SetDriveInput(1);
                    
            }
        }
    }
}