using TMPro;
using UnityEngine;

public class CurrentFloorPresenter : OnLevelLoadedPresenter
{
    public TMP_Text text;
    protected override void UpdateUI()
    {
        text.text = $"Floor {gameManager.CurrentRoom}";
    }
}
