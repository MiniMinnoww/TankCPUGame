using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hurtboxes;

namespace Brains
{
    [Serializable]
    public abstract class BaseCPUBrain : Brain
    {
        private Coroutine moveCoroutine;
        private Coroutine rotateCoroutine;

        #region Constructors
        
        protected BaseCPUBrain() {}
        protected BaseCPUBrain(IPlayerBrainInterface player) : base(player) {}
        
        #endregion

        #region Detecting Objects

        /// <summary>
        /// Returns a list of all detectable objects currently within the tank's field of view.
        /// </summary>
        protected List<IDetectableObject> GetObjectsInViewCone() => player.GetObjectsInViewCone();

        /// <summary>
        /// Returns a list of tanks currently visible within the tank's field of view.
        /// </summary>
        protected List<IDetectableTank> GetTanksInViewCone() => GetObjectsInViewCone().OfType<IDetectableTank>().ToList();

        /// <summary>
        /// Returns a list of projectiles currently visible within the tank's field of view.
        /// </summary>
        protected List<IDetectableProjectile> GetProjectilesInViewCone() => GetObjectsInViewCone().OfType<IDetectableProjectile>().ToList();

        /// <summary>
        /// Returns the closest visible tank to this brain.
        /// </summary>
        protected IDetectableTank GetClosestTank() => GetClosestOfType<IDetectableTank>(ObjectType.Player);

        /// <summary>
        /// Returns the closest visible projectile to this brain.
        /// </summary>
        protected IDetectableProjectile GetClosestProjectile() => GetClosestOfType<IDetectableProjectile>(ObjectType.Projectile);

        /// <summary>
        /// Returns the closest visible object of the specified type.
        /// </summary>
        private T GetClosestOfType<T>(ObjectType type) where T : IDetectableObject
        {
            List<IDetectableObject> objects = GetObjectsInViewCone().Where(obj => obj.GetObjectType() == type).Where(obj => obj != player.GetTankReference()).ToList();
            return objects.Count == 0 ? default : objects.OrderBy(GetDistanceTo).OfType<T>().First();
        }
        
        /// <summary>
        /// Detects if an enemy projectile is in our view cone
        /// </summary>
        /// <returns>Whether the enemy projectile is there or not</returns>
        protected bool IsEnemyProjectileInView()
        {
            return GetClosestProjectile() != null && !IsProjectileOwnedByPlayer(GetClosestProjectile());
        }
        
        /// <summary>
        /// Returns true if there is a tank that would be hit if a bullet was shot this frame
        /// </summary>
        /// <param name="range">The range to search in</param>
        /// <returns>True if there is a shootable tank</returns>
        protected bool IsTankShootableAhead(float range = 10)
        {
            Vector2 origin = player.GetPosition();
            Vector2 forward = player.GetForward();
            RaycastHit2D hit = Physics2D.Raycast(origin + forward, forward, range, LayerMask.GetMask("Hurtbox"));

            return hit.collider && hit.collider.TryGetComponent(out PlayerHurtbox _);
        }
        #endregion

        #region Direction Utilities
        /// <summary>
        /// Returns a normalized direction vector from this tank's position to the target world position.
        /// </summary>
        protected Vector2 GetDirectionTo(Vector2 targetPosition) => (targetPosition - player.GetPosition()).normalized;

        /// <summary>
        /// Returns a normalized direction vector from this tank's position to the specified detectable object.
        /// </summary>
        protected Vector2 GetDirectionTo(IDetectableObject target)  => target == null ? Vector2.zero : GetDirectionTo(target.GetPosition());


        /// <summary>
        /// Returns the direction vector toward the nearest visible tank.
        /// </summary>
        protected Vector2 GetDirectionToNearestTank()
        {
            IDetectableTank closestTank = GetClosestTank();
            return closestTank != null ? GetDirectionTo(closestTank) : Vector2.zero;
        }

        /// <summary>
        /// Returns the direction vector toward the nearest visible projectile.
        /// </summary>
        protected Vector2 GetDirectionToNearestProjectile()
        {
            IDetectableProjectile closestProjectile = GetClosestProjectile();
            return closestProjectile != null ? GetDirectionTo(closestProjectile) : Vector2.zero;
        }
        #endregion

        #region Distance Utilities
        /// <summary>
        /// Returns the distance between this brain and the specified world position.
        /// </summary>
        protected float GetDistanceTo(Vector2 targetPosition) => Vector2.Distance(player.GetPosition(), targetPosition);

        /// <summary>
        /// Returns the distance between this brain and the specified detectable object.
        /// </summary>
        protected float GetDistanceTo(IDetectableObject target)
        {
            return target == null ? float.MaxValue : GetDistanceTo(target.GetPosition());
        }

        /// <summary>
        /// Returns the distance to the nearest visible tank.
        /// </summary>
        protected float GetDistanceToNearestTank()
        {
            IDetectableTank closestTank = GetClosestTank();
            return closestTank != null ? GetDistanceTo(closestTank) : float.MaxValue;
        }

        /// <summary>
        /// Returns the distance to the nearest visible projectile.
        /// </summary>
        protected float GetDistanceToNearestProjectile()
        {
            IDetectableProjectile closestProjectile = GetClosestProjectile();
            return closestProjectile != null ? GetDistanceTo(closestProjectile) : float.MaxValue;
        }
        #endregion

        #region Filtering and Query Helpers
        /// <summary>
        /// Returns true if there is an unobstructed line of sight to the specified target.
        /// </summary>
        protected bool HasLineOfSightTo(IDetectableObject target)
        {
            if (target == null) return false;

            Vector2 origin = player.GetPosition();
            Vector2 targetPos = target.GetPosition();
            Vector2 direction = (targetPos - origin).normalized;
            float distance = Vector2.Distance(origin, targetPos);
            
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Obstacle"));
            if (hit.collider.TryGetComponent(out IDetectableObject obj))
                return obj == target;
            return false;
        }

        /// <summary>
        /// Returns true if the target is within the specified range.
        /// </summary>
        protected bool IsObjectWithinRange(IDetectableObject target, float range)
        {
            if (target == null) return false;
            return GetDistanceTo(target) <= range;
        }

        /// <summary>
        /// Returns true if any tank is currently visible in the view cone
        /// </summary>
        protected bool IsTankVisible()
        {
            List<IDetectableTank> tanks = GetTanksInViewCone();
            return tanks != null && tanks.Count > 0;
        }

        /// <summary>
        /// Returns true if any projectile is currently visible in the view cone.
        /// </summary>
        protected bool IsProjectileVisible()
        {
            List<IDetectableProjectile> projectiles = GetProjectilesInViewCone();
            return projectiles != null && projectiles.Count > 0;
        }

        /// <summary>
        /// Returns true if any detectable object is currently visible.
        /// </summary>
        protected bool IsAnyObjectVisible()
        {
            List<IDetectableProjectile> projectiles = GetProjectilesInViewCone();
            return projectiles != null && projectiles.Count > 0;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Returns true if there is an obstacle directly ahead within the specified range.
        /// </summary>
        protected bool IsObstacleAhead(float range = 10) => GetObstacleAhead(range) != null;

        /// <summary>
        /// Returns the first detectable obstacle directly ahead within the specified range.
        /// </summary>
        protected IDetectableObstacle GetObstacleAhead(float range = 10)
        {
            Vector2 origin = player.GetPosition();
            Vector2 forward = player.GetForward();
            RaycastHit2D hit = Physics2D.Raycast(origin, forward, range, LayerMask.GetMask("Obstacle"));

            return hit.collider == null ? null : hit.collider.GetComponent<IDetectableObstacle>();
        }

        /// <summary>
        /// Sets the drive input for the tank (forward/backward).
        /// Positive values move forward, negative values move backward. Values must be between -1 and 1
        /// </summary>
        protected void SetDriveInput(float value)
        {
            StopMovementCoroutine();
            DriveInput = value;
        }

        /// <summary>
        /// Sets the rotation input for the tank (left/right).
        /// </summary>
        protected void SetRotationInput(float value)
        {
            StopRotationCoroutine();
            RotationInput = value;
        }

        /// <summary>
        /// Stops any current forward or backward movement.
        /// </summary>
        protected void StopDriving()
        {
            StopMovementCoroutine();
            SetDriveInput(0);
        }

        /// <summary>
        /// Stops any current rotational movement.
        /// </summary>
        protected void StopRotating()
        {
            StopRotationCoroutine();
            SetRotationInput(0);
        }

        /// <summary>
        /// Gets the rotation value needed to look at said object. This is either -1, 1 or 0 and can be passed directly into SetRotationInput()
        /// </summary>
        protected float GetRotationToLookAt(IDetectableObject obj, float deadZone = 1f)
        {
            if (obj == null)
                return 0;

            Vector2 selfPos = player.GetPosition();
            Vector2 forward = player.GetForward();
            Vector2 toTarget = (obj.GetPosition() - selfPos).normalized;

            float angle = -Vector2.SignedAngle(forward, toTarget);
            float absAngle = Mathf.Abs(angle);

            if (absAngle < deadZone)
                return 0;

            return Mathf.Clamp(angle / 180f * 1000, -1f, 1f);
        }


        /// <summary>
        /// Moves forward at the given speed for the specified duration (in seconds).
        /// Automatically stops afterward.
        /// </summary>
        protected void MoveForSeconds(float speed, float seconds)
        {
            StopMovementCoroutine();
            moveCoroutine = StartCoroutine(MoveForSecondsRoutine(speed, seconds));
        }

        /// <summary>
        /// Rotates at the given speed for the specified duration (in seconds).
        /// Automatically stops afterward.
        /// </summary>
        protected void RotateForSeconds(float speed, float seconds)
        {
            StopRotationCoroutine();
            rotateCoroutine = StartCoroutine(RotateForSecondsRoutine(speed, seconds));
        }
        
        /// <summary>
        /// Get the current player rotation
        /// </summary>
        /// <returns>The current player rotation</returns>
        protected float GetRotation() => player.GetRotation();

        /// <summary>
        /// Coroutine that drives forward for a fixed duration, then stops.
        /// </summary>
        private IEnumerator MoveForSecondsRoutine(float speed, float seconds)
        {
            SetDriveInput(speed);
            yield return new WaitForSeconds(seconds);
            SetDriveInput(0);
            moveCoroutine = null;
        }

        /// <summary>
        /// Coroutine that rotates for a fixed duration, then stops.
        /// </summary>
        private IEnumerator RotateForSecondsRoutine(float rotation, float seconds)
        {
            SetRotationInput(rotation);
            yield return new WaitForSeconds(seconds);
            SetRotationInput(0);
            rotateCoroutine = null;
        }

        /// <summary>
        /// Cancels any active movement coroutine.
        /// </summary>
        private void StopMovementCoroutine()
        {
            if (moveCoroutine == null) return;
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        /// <summary>
        /// Cancels any active rotation coroutine.
        /// </summary>
        private void StopRotationCoroutine()
        {
            if (rotateCoroutine == null) return;
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }

        /// <summary>
        /// Returns true if the tank is currently moving (driving).
        /// </summary>
        protected bool IsMoving() => DriveInput != 0;

        /// <summary>
        /// Returns true if the tank is currently rotating.
        /// </summary>
        protected bool IsRotating() => RotationInput != 0;

        /// <summary>
        /// Immediately stops all movement and rotation.
        /// </summary>
        protected void StopAllMovement()
        {
            StopMovementCoroutine();
            StopRotationCoroutine();
            SetDriveInput(0);
            SetRotationInput(0);
        }
    #endregion

    #region Utilities
        protected bool IsProjectileOwnedByPlayer(IDetectableProjectile projectile)
        {
            if (projectile != null)
                return projectile.GetOwner() == (IDetectableTank) player;
            return false;
        }
        
        /// <summary>
        /// Gets the health of our player
        /// </summary>
        /// <returns>0 - 1 health of our player</returns>
        protected float GetHealth() => player.GetTankReference().GetHealth();
    #endregion
    }

    public interface IDetectableObject
    {
        /// <summary>
        /// Returns the object type classification of this detectable object.
        /// </summary>
        public ObjectType GetObjectType();

        /// <summary>
        /// Returns the current world position of this detectable object.
        /// </summary>
        public Vector2 GetPosition();
    }

    public interface IDetectableTank : IDetectableObject
    {
        /// <summary>
        /// Gets the health of the player from 0 (no health) to 1 (max health)
        /// </summary>
        /// <returns>0 - 1 health of the player</returns>
        public float GetHealth();
    }
    
    public interface IDetectableProjectile : IDetectableObject
    {
        public IDetectableTank GetOwner();
    }
    
    public interface IDetectableObstacle : IDetectableObject
    {
        
    }

    public enum ObjectType
    {
        Player,
        Projectile,
        Obstacle
    }
}
