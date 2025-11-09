using Brains;
using UnityEngine;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour, IDetectableObstacle
    {
        public ObjectType GetObjectType() => ObjectType.Obstacle;
        public Vector2 GetPosition() => transform.position;
    }
}