using System;

namespace Brains.Brains
{
    [Serializable]
    public class DumbassBrain : Brain 
    {
        public DumbassBrain() {}
        public DumbassBrain(IPlayerBrainInterface player) : base(player) { }

        public override void Update() 
        {
            DriveInput = 1;
            RotationInput = 1;
        }
    }
}