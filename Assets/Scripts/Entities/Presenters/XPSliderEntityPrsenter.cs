using UnityEngine;
using UnityEngine.UI;

public class XPSliderEntityPrsenter : EntityDataPresenter
{
    [Header("XP slider")]
    [SerializeField] Slider XPSlider;
    [SerializeField] Color HPColorEmpty = Color.white;
    [SerializeField] Color HPColorFull = Color.white;
    protected override void UpdateDisplay()
    {
        SetSlider(XPSlider, boundEntity.CurrentXP, boundEntity.XPRequired);
        SetSliderColor(XPSlider, HPColorEmpty, HPColorFull);
    }
}
