using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class InputBrain : Brain 
{
    // Didn't bother using the new Input System

    public override void Update() 
    {
        DriveInput = Input.GetAxis("Vertical");
        RotationInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }
}