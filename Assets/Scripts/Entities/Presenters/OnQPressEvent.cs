using System;
using UnityEngine;

public class OnQPressEvent : MonoBehaviour
{
    public static event Action<bool> OnQPress;
    public static bool currentState = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = !currentState;
            OnQPress?.Invoke(currentState);
        }
    }
}
