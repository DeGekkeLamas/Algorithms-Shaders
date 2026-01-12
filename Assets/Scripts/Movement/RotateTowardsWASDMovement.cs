using UnityEngine;

/// <summary>
/// Rotates transform towards WASD input
/// </summary>
public class RotateTowardsWASDMovement : MonoBehaviour
{
    void Update()
    {
        float camAngle = Camera.main.transform.eulerAngles.y;
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = VectorMath.RotateVectorXZ(movement, -camAngle);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }
}
