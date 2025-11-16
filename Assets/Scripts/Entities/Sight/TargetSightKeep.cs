using UnityEngine;

namespace MovementStuff
{
    /// <summary>
    /// Switches between movement types depending on if target can be seen by object, 
    /// switches back when target cant be seen anymore
    /// </summary>
    public class TargetSightKeep : TargetSight
    {
        bool hasSeenBefore;
        protected void Update()
        {
            if (!hasSeenBefore) InvokeOnFirstSeenTarget();
            hasSeenBefore = true;
            bool canSeePlayer = CanSeePlayer(this.transform, target.transform, maxVisionDistance, visionAngle);
            MovementBeforeSeenTarget.enabled = !canSeePlayer;
            MovementAfterSeenTarget.enabled = canSeePlayer;
        }
    }
}
