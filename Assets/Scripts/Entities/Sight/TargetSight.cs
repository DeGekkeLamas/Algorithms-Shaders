using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System;

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

    public bool showDebug;
    public event Action OnFirstSeenTarget;

    private void OnValidate()
    {
        if (showDebug) StartCoroutine(ShowDebug());
    }

    private void Start()
    {
        MovementBeforeSeenTarget.enabled = true;
        MovementAfterSeenTarget.enabled = false;
        if (target == null) target = PlayerController.instance.transform;
    }

    protected void InvokeOnFirstSeenTarget()
    {
        OnFirstSeenTarget?.Invoke();
    }


    /// <summary>
    /// Returns if the object has sight of the player, based off distance, angle and objects in the way
    /// </summary>
    /// <returns></returns>
    public static bool CanSeePlayer(Transform self, Transform target, float maxVisionDistance, float visionAngle)
    {

        return PlayerIsInRange(self, target, maxVisionDistance, visionAngle) &&
            Physics.Raycast(self.transform.position + Vector3.up, target.position + Vector3.up - (self.transform.position), // Direct line of sight
            out RaycastHit visionHit) && visionHit.collider.transform == target;                                         // to target
    }
    public static bool PlayerIsInRange(Transform self, Transform target, float maxVisionDistance, float visionAngle)
    {
        Vector3 targetPos = target.position;
        float targetEnemyDistance = (targetPos - self.transform.position).magnitude;

        return targetEnemyDistance > .5f && targetEnemyDistance < maxVisionDistance && // Distance
            VectorMath.GetAngleBetweenVectors(targetPos - self.transform.position, self.transform.forward) < visionAngle; // Vision angle
    }

    IEnumerator ShowDebug()
    {
        while (showDebug)
        {
            VisualDebug();
            yield return null;
        }
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
