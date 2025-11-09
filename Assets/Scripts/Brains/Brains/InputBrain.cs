using System;
using UnityEngine;

namespace Brains.Brains
{
    [Serializable]
    public class InputBrain : Brain 
    {
        // Didn't bother using the new Input System 
        public InputBrain() {}
        public InputBrain(IPlayerBrainInterface player) : base(player) { }

        public override void Update() 
        {
            DriveInput = Input.GetAxis("Vertical");
            RotationInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space)) Shoot();
        }
    }
}