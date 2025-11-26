using UnityEngine;

public abstract class ExitDoorPresenter : MonoBehaviour
{
    protected ExitDoor exit;
    private void Awake()
    {
        exit = this.GetComponent<ExitDoor>();
    }
    private void OnEnable()
    {
        exit.OnStatsUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        exit.OnStatsUpdated -= UpdateUI;
    }

    protected abstract void UpdateUI();
}
