using UnityEngine;

[CreateAssetMenu(menuName="Game/Tank Movement Stats")]
public class TankMovementStats : ScriptableObject 
{
    public float moveSpeed;
    public float rotationSpeed;
    public float extraRotationWhenMoving;
}