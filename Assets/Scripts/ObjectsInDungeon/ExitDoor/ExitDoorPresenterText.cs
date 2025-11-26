using TMPro;
using UnityEngine;

public class ExitDoorPresenterText : ExitDoorPresenter
{
    public TMP_Text text;
    protected override void UpdateUI()
    {
        if (exit.IsOpen)
        {
            text.text = "Door is open";
            return;
        }
        text.text = $"{exit.killsRequired - exit.KillsDone} enemies left";
    }
}
