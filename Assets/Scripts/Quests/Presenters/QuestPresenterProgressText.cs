using TMPro;
using UnityEngine;

namespace Quests.Presenters
{
    public class QuestPresenterProgressText : QuestPresenterOnProgressUpdated
    {
        TMP_Text text;

        private void Awake()
        {
            text = this.GetComponent<TMP_Text>();
        }
        protected override void UpdateDisplay()
        {
            if (boundQuest.progress >= boundQuest.maxProgress)
            {
                text.text = $"Complete!";
            }
            else
            {
                text.text = $"{boundQuest.progress} / {boundQuest.maxProgress}";
            }
        }
    }
}
