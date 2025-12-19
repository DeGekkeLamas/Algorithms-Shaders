using UnityEngine;

namespace MovementStuff
{
    public class WASDMovement : MovingObjectBase
    {
        [SerializeField] float moveSpeed = 1;
        void Update()
        {
            // Camera angle to move relative to it
            float camAngle = Camera.main.transform.eulerAngles.y;

            Vector3 movement = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movement = VectorMath.RotateVectorXZ(movement, -camAngle);

            transform.position += moveSpeed * baseSpeed * Time.deltaTime * movement;
        }
    }
}
