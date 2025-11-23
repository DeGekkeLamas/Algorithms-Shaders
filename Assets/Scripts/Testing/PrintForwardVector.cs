using NaughtyAttributes;
using UnityEngine;

public class PrintForwardVector : MonoBehaviour
{
    [Button]
    void PrintForward()
    {
        Debug.Log(transform.forward);
    }
}
