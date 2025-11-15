using UnityEngine;

/// <summary>
/// Quest presenter for quest details that will be updated as the quest progresses
/// </summary>
public abstract class QuestPresenterOnProgressUpdated : QuestPresenter
{
    private void Start()
    {
        boundQuest.OnProgressUpdated += UpdateDisplay;
    }
    private void OnDestroy()
    {
        boundQuest.OnProgressUpdated -= UpdateDisplay;
    }
}
