using UnityEngine;
using UnityEngine.UI;

public class HPSliderEntityPrsenter : TextBasedEntityPresenter
{
    [Header("HP slider")]
    [SerializeField] Slider HPSlider;
    [SerializeField] Color XPColorEmpty = Color.white;
    [SerializeField] Color XPColorFull = Color.white;

    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();
        // HP slider
        SetSlider(HPSlider, boundEntity.CurrentHP, boundEntity.maxHP);
        SetSliderColor(HPSlider, XPColorEmpty, XPColorFull);
    }
}
