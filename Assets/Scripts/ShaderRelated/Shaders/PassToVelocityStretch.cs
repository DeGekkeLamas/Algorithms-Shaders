using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PassToVelocityStretch : MonoBehaviour
{
    Rigidbody r;
    Material velocityStretch;
    private void Awake()
    {
        r = GetComponent<Rigidbody>();
        velocityStretch = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        velocityStretch.SetVector("_Velocity", r.linearVelocity);

        Vector3 rotationRadians = VectorMath.EulerToRadians(this.transform.eulerAngles);
        velocityStretch.SetVector("_Rotation", VectorMath.EulerToRadians(this.transform.eulerAngles));

        Vector3 velocityAngle = Quaternion.LookRotation(r.linearVelocity).eulerAngles;
        velocityStretch.SetVector("_VelocityRotation", -VectorMath.EulerToRadians(velocityAngle));
    }
}
