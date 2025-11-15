using UnityEngine;

/// <summary>
/// This baseclass does NOT add functions to update displays to any events, override it to add that functionality
/// </summary>
public abstract class QuestPresenter : MonoBehaviour
{
    public Quest boundQuest;

    protected abstract void UpdateDisplay();
}
