using UnityEngine;

public abstract class QuestDoorPresenter : MonoBehaviour
{

    protected QuestDoor door;
    private void Awake()
    {
        door = this.GetComponent<QuestDoor>();
    }
    private void OnEnable()
    {
        door.OnStatsUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        door.OnStatsUpdated -= UpdateUI;
    }

    protected abstract void UpdateUI();
}
