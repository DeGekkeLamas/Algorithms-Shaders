using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MathTools
{
    /// <summary>
    /// Adds up all elements of an array together
    /// </summary>
    public static int ArrayTotal(int[] array)
    {
        int total = 0;
        foreach (var item in array)
        {
            total += item;
        }
        return total;
    }
    /// <summary>
    /// Adds up all elements of an array together
    /// </summary>
    public static float ArrayTotal(float[] array)
    {
        float total = 0;
        foreach (var item in array)
        {
            total += item;
        }
        return total;
    }
    /// <summary>
    /// Remaps a certain range into anither one
    /// </summary>
    public static float Remap(float oldRangeX, float oldRangeY, float newRangeX, float newRangeY, float value)
    {
        value = Mathf.InverseLerp(oldRangeX, oldRangeY, value);
        return Mathf.Lerp(newRangeX, newRangeY, value);
    }

    /// <summary>
    /// Returns a list of every child of this gameobject, also gives children of children
    /// </summary>
    public static List<GameObject> GetAllChildren(GameObject origin)
    {
        List<GameObject> found = new();

        for (int i = 0; i < origin.transform.childCount ; i++)
        {
            GameObject child = origin.transform.GetChild(i).gameObject;
            found.Add(child);

            foreach(GameObject deepChild in GetAllChildren(child))
            {
                found.Add(deepChild);
            }
        }

        return found;
    }

    public static T GetComponentInParents<T>(Transform target) where T : Component
    {
        // Get T
        Transform current = target;
        while (current != null) // find canvas in parents
        {
            if (current.TryGetComponent(out T found))
            {
                return found;
            }
            current = current.parent;
        }
        Debug.LogError("Could not find canvas");
        return null;
    }

    public static Vector3 Vector3Multiply(Vector3 a, Vector3 b)
    {
        return new(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static Vector3 Vector3Divide(Vector3 a, Vector3 b)
    {
        return new(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    public static Vector3 Vector3Round(Vector3 input)
    {
        return new(Mathf.Round(input.x), Mathf.Round(input.y), Mathf.Round(input.z));
    }

    public static Vector3 Vector3Modulo(Vector3 input, Vector3 amount)
    {
        return new(input.x % amount.x, input.y % amount.y, input.z % amount.z);
    }

    public static Vector3 Vector3Clamp(Vector3 input, Vector3 min, Vector3 max)
    {
        return new( Mathf.Clamp(input.x, min.x, max.x), Mathf.Clamp(input.y, min.y, max.y), Mathf.Clamp(input.z, min.z, max.z) );
    }

    public static Vector3 Vector3Min(Vector3 input, Vector3 min)
    {
        return new( Mathf.Min(input.x, min.x), Mathf.Min(input.y, min.y), Mathf.Min(input.z, min.z) );
    }

    public static Vector3 Vector3Max(Vector3 input, Vector3 max)
    {
        return new( Mathf.Max(input.x, max.x), Mathf.Max(input.y, max.y), Mathf.Max(input.z, max.z) );
    }

    public static Vector3 Vector3Abs(Vector3 input)
    {
        return new(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
    }

    public static float Vector3CompSum(Vector3 input)
    {
        return input.x + input.y + input.z;
    }
}
