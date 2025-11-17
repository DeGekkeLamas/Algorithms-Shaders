using UnityEngine;

namespace MovementStuff
{
    public class MovingObject : MovingObjectBase
    {
        /// <summary>
        /// Script intended to be placed on enemies, though it can be placed on other moving obstacles as well
        /// </summary>

        [Header("Movement related")]
        public Vector3 moveSpeed;
        public Vector3 moveRange;
        public Vector3 offset;

        Vector3 _oriPos;

        public enum MovementType { Circular, PingPong, Forward };
        public MovementType currentMovement = MovementType.PingPong;

        void Awake()
        {
            _oriPos = this.transform.localPosition;
        }

        void FixedUpdate()
        {
            // Moves object
            switch (currentMovement)
            {
                case MovementType.PingPong:
                    MovementPingPong();
                    break;
                case MovementType.Circular:
                    MovementCircular();
                    break;
                case MovementType.Forward:
                    MovementForward();
                    break;
            }
        }
        void MovementPingPong()
        {
            this.transform.localPosition = _oriPos + new Vector3(
                            Mathf.Sin(baseSpeed * moveSpeed.x * Time.time + offset.x) * moveRange.x,
                            Mathf.Sin(baseSpeed * moveSpeed.y * Time.time + offset.y) * moveRange.y,
                            Mathf.Sin(baseSpeed * moveSpeed.z * Time.time + offset.z) * moveRange.z);
        }
        void MovementCircular()
        {
            this.transform.localPosition = _oriPos + new Vector3(
                            Mathf.Sin(baseSpeed * moveSpeed.x * Time.time + offset.x) * moveRange.x,
                            Mathf.Sin(baseSpeed * moveSpeed.y * Time.time + offset.y) * moveRange.y,
                            Mathf.Cos(baseSpeed * moveSpeed.z * Time.time + offset.z) * moveRange.z);
        }
        void MovementForward()
        {
            this.transform.localPosition = _oriPos + new Vector3(
                            baseSpeed * moveSpeed.x * Time.time,
                            baseSpeed * moveSpeed.y * Time.time,
                            baseSpeed * moveSpeed.z * Time.time);
        }
    }
}
