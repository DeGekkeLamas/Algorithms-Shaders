using NaughtyAttributes;
using UnityEngine;

public class MoveTowardsTarget : MovingObjectBase
{

    [InfoBox("Leave target empty for it to be automatically set to player instance")]
    public Transform target;
    public float moveSpeed = 1;
    public float rotationSpeed = 1;
    [Tooltip("this object will never get closer than the minimum distance")]
    public float minDistance = 0;

    private void Start()
    {
        if (target == null) target = PlayerController.instance.transform;
    }
    private void Update()
    {
        transform.LookAt(target);
        Vector3 diff = this.transform.position - target.position;
        float distance = diff.magnitude;

        if (distance > minDistance)
        {
            this.transform.position -= baseSpeed * moveSpeed * Time.deltaTime * (diff / distance);
        }
        else
        {
            this.transform.position += baseSpeed * moveSpeed * Time.deltaTime * (diff / distance);
        }
        
    }
}
