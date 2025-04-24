using UnityEngine;

public class VectorMath
{
    /// <summary>
    /// Rotation over Y-axis
    /// </summary>
    public static Vector3 RotateVectorXZ(Vector3 start, float rotation)
    {
        rotation = DegreesToRadians(rotation);

        return new(
                start.x * Mathf.Cos(rotation) - start.z * Mathf.Sin(rotation)
                , start.y
                , start.x * Mathf.Sin(rotation) + start.z * Mathf.Cos(rotation)
            );
    }
    /// <summary>
    /// Rotation over X-axis
    /// </summary>
    public static Vector3 RotateVectorYZ(Vector3 start, float rotation)
    {
        rotation = DegreesToRadians(rotation);

        return new(
                start.x,
                start.y * Mathf.Cos(rotation) - start.z * Mathf.Sin(rotation)
                , start.y * Mathf.Sin(rotation) + start.z * Mathf.Cos(rotation)
            );
    }
    /// <summary>
    /// Rotation over Z-axis
    /// </summary>
    public static Vector3 RotateVectorXY(Vector3 start, float rotation)
    {
        rotation = DegreesToRadians(rotation);

        return new(
                start.x * Mathf.Cos(rotation) - start.y * Mathf.Sin(rotation)
                , start.x * Mathf.Sin(rotation) + start.y * Mathf.Cos(rotation)
                , start.z
            );
    }
    public static Vector3 RotateVector3(Vector3 originalVector, Vector3 rotation)
    {
        Vector3 newVector = Quaternion.Euler(rotation) * originalVector;
        return newVector;
    }
    public static float GetAngleBetweenVectors(Vector3 dir1, Vector3 dir2)
    {
        float _angle = Mathf.Acos(Vector3.Dot(dir2, dir1) / (dir2.magnitude * dir1.magnitude)) * (180 / Mathf.PI);
        return _angle;
    }
    public static float DegreesToRadians(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }
    public static Vector3 EulerToRadians(Vector3 eulerAngles)
    {
        return eulerAngles * Mathf.PI / 180;
    }
}
