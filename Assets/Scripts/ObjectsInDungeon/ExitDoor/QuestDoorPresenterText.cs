using TMPro;
using UnityEngine;

public class QuestDoorPresenterText : QuestDoorPresenter
{
    public TMP_Text text;
    protected override void UpdateUI()
    {
        if (door.IsOpen)
        {
            text.text = "Door is open";
            return;
        }
        text.text = $"{door.questsRequired.Length - door.QuestsDone} quests left";
    }
}
