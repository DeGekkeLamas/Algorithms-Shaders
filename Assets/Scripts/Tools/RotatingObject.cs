using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementStuff
{
    public enum RotationType { Linear, PingPong, AltPingPong };

    /// <summary>
    /// Can be attached to any object that should be rotated
    /// </summary>
    public class RotatingObject : MovingObjectBase
    {
        Vector3 _oriRot;

        [SerializeField] RotationType currentRotation = RotationType.Linear;
        [SerializeField] bool rotatesWithParent;

        [Header("Speeds")]
        [SerializeField] Vector3 rotationSpeed;
        [SerializeField] Vector3 rotationRange;

        void Awake() => _oriRot = transform.eulerAngles;

        void Update()
        {
            if (rotatesWithParent) _oriRot = transform.parent.eulerAngles;
            switch (currentRotation)
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
            transform.eulerAngles = _oriRot + Time.time * rotationSpeed * baseSpeed;
        }
        void PingPongRotation()
        {
            transform.eulerAngles = _oriRot + new Vector3(
                Mathf.Sin(Time.time * rotationSpeed.x * baseSpeed) * rotationRange.x,
                Mathf.Sin(Time.time * rotationSpeed.y * baseSpeed) * rotationRange.y,
                Mathf.Sin(Time.time * rotationSpeed.z * baseSpeed) * rotationRange.z);
        }                                                             
        void AltPingPongRotation()
        {
            transform.eulerAngles = _oriRot + new Vector3(
                Mathf.Cos(Time.time * rotationSpeed.x * baseSpeed) * rotationRange.x,
                Mathf.Cos(Time.time * rotationSpeed.y * baseSpeed) * rotationRange.y,
                Mathf.Cos(Time.time * rotationSpeed.z * baseSpeed) * rotationRange.z);
        }
    }
}
