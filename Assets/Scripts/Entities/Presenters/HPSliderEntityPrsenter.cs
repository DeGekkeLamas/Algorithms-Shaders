using UnityEngine;
using UnityEngine.UI;

namespace Entities.Presenters
{
    public class HPSliderEntityPrsenter : EntityDataPresenter
    {
        [Header("HP slider")]
        [SerializeField] Slider HPSlider;
        [SerializeField] Color XPColorEmpty = Color.white;
        [SerializeField] Color XPColorFull = Color.white;
        protected override void UpdateDisplay()
        {
            SetSlider(HPSlider, boundEntity.CurrentHP, boundEntity.maxHP);
            SetSliderColor(HPSlider, XPColorEmpty, XPColorFull);
        }
    }
}
