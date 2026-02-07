using UnityEngine;

public class InvertActive : MonoBehaviour
{
    public void ToggleActive()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
    public void ToggleActive(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
}
