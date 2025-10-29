using System;
using UnityEngine;

public abstract class Brain
{
    public Action onShoot;
    private float driveInput;

    public float DriveInput
    {
        get => driveInput;
        set => driveInput = Mathf.Clamp(value, -1, 1);
    }

    private float rotationInput;

    public float RotationInput
    {
        get => rotationInput;
        set => rotationInput = Mathf.Clamp(value, -1, 1);
    }

    public virtual void Update() { }
    
    protected void Shoot() => onShoot?.Invoke();
}