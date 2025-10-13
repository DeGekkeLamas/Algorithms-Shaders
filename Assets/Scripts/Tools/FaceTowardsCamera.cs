using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Aligns transform to the camera.
/// </summary>
public class FaceTowardsCamera : MonoBehaviour
{
    void Update()
    {
        FaceToCamera();
    }
    [Button]
    void FaceToCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}