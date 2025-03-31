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
    public static float GetAngleBetweenVectors(Vector3 start, Vector3 end)
    {
        float _angle = Mathf.Acos(Vector3.Dot(end, end - start) / (end.magnitude * (end - start).magnitude)) * (180 / Mathf.PI);
        return _angle;
    }
}
