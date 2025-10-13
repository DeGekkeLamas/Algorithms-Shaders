using UnityEngine;
using UnityEngine.UI;

public class XPHPSliderEntityPrsenter : HPSliderEntityPrsenter
{
    [Header("XP slider")]
    [SerializeField] Slider XPSlider;
    [SerializeField] Color HPColorEmpty = Color.white;
    [SerializeField] Color HPColorFull = Color.white;
    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();
        // XP slider
        SetSlider(XPSlider, boundEntity.CurrentXP, boundEntity.XPRequired);
        SetSliderColor(XPSlider, HPColorEmpty, HPColorFull);
    }
}
