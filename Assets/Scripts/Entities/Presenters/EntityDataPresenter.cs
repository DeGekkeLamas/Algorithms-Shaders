using UnityEngine;
using UnityEngine.UI;

public abstract class EntityDataPresenter : MonoBehaviour
{
    protected Entity boundEntity;

    private void Start()
    {
        boundEntity = GetComponent<Entity>();
        boundEntity.onStatsChanged += UpdateDisplay;
    }

    protected void SetSlider(Slider slider, float value, float maxValue)
    {
        slider.value = value/ maxValue * slider.maxValue;
    }
    protected void SetSliderColor(Slider slider, Color empty, Color full)
    {
        slider.fillRect.GetComponent<Image>().color = Color.Lerp(empty, full, slider.value/slider.maxValue);
    }
    protected abstract void UpdateDisplay();
}
