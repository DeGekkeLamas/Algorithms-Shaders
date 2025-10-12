using UnityEngine;

/// <summary>
/// Switch between movement types once the target has been seen for the first time
/// </summary>
public class TargetSightOnce : TargetSight
{
    protected override void Update()
    {
        base.Update();
        if (CanSeePlayer(this.transform, target.transform, maxVisionDistance, visionAngle))
        {
            MovementBeforeSeenTarget.enabled = false;
            MovementAfterSeenTarget.enabled = true;
        }
    }
}
