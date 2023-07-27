using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HerosJourney.Utils
{
    public class SettingsSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Slider _slider;

        public void UpdateValueText() => _valueText.SetText(_slider.value.ToString());
    }
}