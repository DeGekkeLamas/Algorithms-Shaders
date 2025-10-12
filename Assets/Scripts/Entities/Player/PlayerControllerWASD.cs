using UnityEngine;

/// <summary>
/// Playercomtroller that uses WASD for movement
/// </summary>
public class PlayerControllerWASD : PlayerController
{
    protected override void Update()
    {
        // Camera angle to move relative to it
        float camAngle = Camera.main.transform.eulerAngles.y;

        Vector3 movement = new(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        movement = VectorMath.RotateVectorXZ(movement, -camAngle);

        transform.position += moveSpeed * Time.deltaTime * movement;
        
        UpdateAction(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetKeyDown(KeyCode.E));
    }
}
