using UnityEngine;


namespace Quests.Presenters
{
    /// <summary>
    /// Quest presenter for stats that will always remain the same and dont need to be updated after the quest is first initialized
    /// </summary>
    public abstract class QuestPresenterOnInitialize : QuestPresenter
    {
        private void Start()
        {
            boundQuest.OnInitialize += UpdateDisplay;
        }
        private void OnDestroy()
        {
            boundQuest.OnInitialize -= UpdateDisplay;
        }
    }
}
