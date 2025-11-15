using UnityEngine;
using UnityEngine.UI;

public class QuestPresenterSlider : QuestPresenterOnProgressUpdated
{
    Slider slider;

    private void Awake()
    {
        slider = this.GetComponent<Slider>();
    }
    protected override void UpdateDisplay()
    {
        slider.value = (float)boundQuest.progress / boundQuest.maxProgress;
    }
}
