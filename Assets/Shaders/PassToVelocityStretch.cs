using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PassToVelocityStretch : MonoBehaviour
{
    Rigidbody r;
    Material[] velocityStretch;
    private void Awake()
    {
        r = GetComponent<Rigidbody>();
        velocityStretch = GetComponent<MeshRenderer>().materials;
    }
    void Update()
    {
        foreach(Material mat in velocityStretch)
        {
            mat.SetVector("_Velocity", r.linearVelocity);
        }
    }
}
