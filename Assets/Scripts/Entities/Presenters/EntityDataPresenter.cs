using UnityEngine;
using UnityEngine.UI;

namespace Entities.Presenters
{
    public abstract class EntityDataPresenter : MonoBehaviour
    {
        protected Entity boundEntity;

        private void Awake()
        {
            boundEntity = GetComponent<Entity>();
        }

        protected static void SetSlider(Slider slider, float value, float maxValue)
        {
            slider.value = value / maxValue * slider.maxValue;
        }
        protected static void SetSliderColor(Slider slider, Color empty, Color full)
        {
            slider.fillRect.GetComponent<Image>().color = Color.Lerp(empty, full, slider.value / slider.maxValue);
        }
        protected abstract void UpdateDisplay();

        private void OnEnable()
        {
            boundEntity.OnStatsChanged += UpdateDisplay;
        }
        private void OnDisable()
        {
            boundEntity.OnStatsChanged -= UpdateDisplay;
        }
    }
}
