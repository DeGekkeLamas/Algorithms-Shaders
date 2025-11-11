using UnityEngine;

/// <summary>
/// Quest presenter for quest details that will be updated as the quest progresses
/// </summary>
public abstract class QuestPresenterOnProgressUpdated : MonoBehaviour
{
    public Quest boundQuest;

    protected abstract void UpdateDisplay();

    private void OnEnable()
    {
        boundQuest.OnProgressUpdated += UpdateDisplay;
    }
    private void OnDisable()
    {
        boundQuest.OnProgressUpdated -= UpdateDisplay;
    }
}
