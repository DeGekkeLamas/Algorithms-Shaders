using UnityEngine;

public class VectorMath
{
    public static Vector3 RotateVectorXZ(Vector3 start, float rotation)
    {
        rotation *= (Mathf.PI / 180);

        return new(
                start.x * Mathf.Cos(rotation) - start.z * Mathf.Sin(rotation)
                , start.y
                , start.x * Mathf.Sin(rotation) + start.z * Mathf.Cos(rotation)
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
}
