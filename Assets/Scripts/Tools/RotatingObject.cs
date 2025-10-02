using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    /// <summary>
    /// Can be attached to any object that should be rotated
    /// </summary>
    Vector3 _oriRot;

    public enum RotationType { Linear, PingPong, AltPingPong};
    public RotationType currentRotation = RotationType.Linear;
    public bool rotatesWithParent;

    [Header("Speeds")]
    public float RotationSpeedX;
    public float RotationSpeedY;
    public float RotationSpeedZ;
    public float RotationRangeX;
    public float RotationRangeY;
    public float RotationRangeZ;

    // Start is called before the first frame update
    void Start() => _oriRot = transform.eulerAngles;

    // Update is called once per frame
    void Update()
    {
        if (rotatesWithParent) _oriRot = transform.parent.eulerAngles;
        switch(currentRotation)
        {
            case RotationType.Linear:
                LinearRotation();
                break;
            case RotationType.PingPong:
                PingPongRotation();
                break;
            case RotationType.AltPingPong:
                AltPingPongRotation();
                break;
        }
    }
    void LinearRotation()
    {
        transform.eulerAngles = _oriRot + new Vector3(
            Time.time * RotationSpeedX,
            Time.time * RotationSpeedY,
            Time.time * RotationSpeedZ);
    }
    void PingPongRotation()
    {
        transform.eulerAngles = _oriRot + new Vector3(
            Mathf.Sin(Time.time * RotationSpeedX) * RotationRangeX,
            Mathf.Sin(Time.time * RotationSpeedY) * RotationRangeY,
            Mathf.Sin(Time.time * RotationSpeedZ) * RotationRangeZ);
    }
    void AltPingPongRotation()
    {
        transform.eulerAngles = _oriRot + new Vector3(
            Mathf.Cos(Time.time * RotationSpeedX) * RotationRangeX,
            Mathf.Cos(Time.time * RotationSpeedY) * RotationRangeY,
            Mathf.Cos(Time.time * RotationSpeedZ) * RotationRangeZ);
    }
}
