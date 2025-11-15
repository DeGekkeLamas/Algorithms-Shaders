using TMPro;
using UnityEngine;

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
