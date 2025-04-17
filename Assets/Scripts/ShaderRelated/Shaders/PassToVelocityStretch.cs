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
        velocityStretch.SetVector("_Velocity", this.transform.InverseTransformDirection(r.linearVelocity));
    }
}
