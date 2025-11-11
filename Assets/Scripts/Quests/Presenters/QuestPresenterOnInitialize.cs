using UnityEngine;

/// <summary>
/// Quest presenter for stats that will always remain the same and dont need to be updated after the quest is first initialized
/// </summary>
public abstract class QuestPresenterOnInitialize : MonoBehaviour
{
    public Quest boundQuest;

    protected abstract void SetDisplay();

    private void OnEnable()
    {
        boundQuest.OnInitialize += SetDisplay;
    }
    private void OnDisable()
    {
        boundQuest.OnInitialize -= SetDisplay;
    }
}
