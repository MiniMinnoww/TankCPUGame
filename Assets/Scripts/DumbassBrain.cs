using System;

[Serializable]
public class DumbassBrain : Brain 
{
    public override void Update() 
    {
        DriveInput = 1;
        RotationInput = 1;
    }
}