using UnityEngine;

namespace Quests.Presenters
{
    /// <summary>
    /// This baseclass does NOT add functions to update displays to any events, override it to add that functionality.
    /// This is on purpose as some events should be updated only once and others every time progress is updated
    /// </summary>
    public abstract class QuestPresenter : MonoBehaviour
    {
        public Quest boundQuest;

        protected abstract void UpdateDisplay();
    }
}
