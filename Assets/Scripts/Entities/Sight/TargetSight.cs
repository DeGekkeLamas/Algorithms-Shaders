using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Base for switching between movement types depending on if target can be seen by object
/// </summary>
public abstract class TargetSight : MonoBehaviour
{
    public MovingObjectBase MovementBeforeSeenTarget;
    public MovingObjectBase MovementAfterSeenTarget;
    [InfoBox("Leave target empty for it to be automatically set to player instance")]
    public Transform target;
    [Header("Vision")]
    public float maxVisionDistance = 15;
    public float visionAngle = 75;

    private void Awake()
    {
        MovementBeforeSeenTarget.enabled = true;
        MovementAfterSeenTarget.enabled = false;
    }
    private void Start()
    {
        if (target == null) target = PlayerController.instance.transform;
    }

    protected virtual void Update()
    {
        if (target == null) return;
        VisualDebug();
    }

    /// <summary>
    /// Returns if the object has sight of the player, based off distance, angle and objects in the way
    /// </summary>
    /// <returns></returns>
    protected bool CanSeePlayer()
    {
        Vector3 targetPos = target.position;
        float targetEnemyDistance = (targetPos - this.transform.position).magnitude;

        return targetEnemyDistance > .5f && targetEnemyDistance < maxVisionDistance && // Distance
            VectorMath.GetAngleBetweenVectors(targetPos - this.transform.position, this.transform.forward) < visionAngle && // Vision angle
                Physics.Raycast(this.transform.position + Vector3.up, targetPos + Vector3.up - (this.transform.position), // Direct line of sight
            out RaycastHit visionHit) && visionHit.collider.transform == target;                                         // to target
    }

    /// <summary>
    /// shows vision range
    /// </summary>
    void VisualDebug()
    {
        DebugExtension.DebugCircle(this.transform.position, Color.green, maxVisionDistance);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxVisionDistance, visionAngle), Color.green);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxVisionDistance, -visionAngle), Color.green);
    }
}
