using UnityEngine;

namespace MovementStuff
{
    /// <summary>
    /// Switch between movement types once the target has been seen for the first time
    /// </summary>
    public class TargetSightOnce : TargetSight
    {
        protected void Update()
        {
            if (CanSeeTarget(this.transform, target.transform, maxVisionDistance, visionAngle))
            {
                MovementBeforeSeenTarget.enabled = false;
                MovementAfterSeenTarget.enabled = true;
                InvokeOnFirstSeenTarget();
                // Destroy as it will no longer be needed
                Destroy(this);
            }
        }
    }
}
