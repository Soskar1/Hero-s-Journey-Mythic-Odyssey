using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HerosJourney.Core.UI
{
    public class RenderDistanceSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        private void Awake()
        {
            _slider.onValueChanged.AddListener((value) =>
            {
                _valueText.text = value.ToString();
            });
        }
    }
}