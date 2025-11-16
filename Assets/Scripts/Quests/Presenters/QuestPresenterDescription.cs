using TMPro;
using UnityEngine;

namespace Quests.Presenters
{
    public class QuestPresenterDescription : QuestPresenterOnInitialize
    {
        TMP_Text text;
        private void Awake()
        {
            text = this.GetComponent<TMP_Text>();
        }
        protected override void UpdateDisplay()
        {
            text.text = boundQuest.description;
        }
    }
}
