using UnityEngine;

namespace MovementStuff
{
    /// <summary>
    /// Switch between movement types once the target has been seen for the first time
    /// </summary>
    public class TargetSightOnce : TargetSight
    {
        bool hasSeenBefore;
        protected void Update()
        {
            if (CanSeePlayer(this.transform, target.transform, maxVisionDistance, visionAngle))
            {
                MovementBeforeSeenTarget.enabled = false;
                MovementAfterSeenTarget.enabled = true;
                if (!hasSeenBefore) InvokeOnFirstSeenTarget();
                hasSeenBefore = true;

            }
        }
    }
}
