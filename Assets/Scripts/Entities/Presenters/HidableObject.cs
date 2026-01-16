using UnityEngine;

public class HidableObject : MonoBehaviour
{
    private void Start()
    {
        OnQPressEvent.OnQPress += InvertState;
        InvertState(OnQPressEvent.currentState);
    }

    private void OnDestroy()
    {
        OnQPressEvent.OnQPress -= InvertState;
    }

    void InvertState(bool state)
    {
        this.gameObject.SetActive(state);
    }
}
