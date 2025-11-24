namespace Brains.Brains
{
    public class CustomiseCPUBrain : BaseCPUBrain
    {
        // Ignore these, don't delete them
        public CustomiseCPUBrain() {}

        public CustomiseCPUBrain(IPlayerBrainInterface player) : base(player) {}
        
        // Put any global variables under here
        
        
        // This function runs every frame
        public override void Update()
        {
            // TODO: Rewrite this code with your own implementation
            
            // If there is a tank in our view, rotate towards it
            // If the tank is shootable, then shoot
            IDetectableTank closestTank = GetClosestTank();
            if (closestTank != null)
            {
                float targetRotation = GetRotationToLookAt(closestTank);
                SetRotationInput(targetRotation);
                
                if (IsTankShootableAhead()) Shoot();
            }
        }
    }
}