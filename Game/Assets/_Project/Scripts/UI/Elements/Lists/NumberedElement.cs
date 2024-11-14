using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Elements.Lists
{
    public class NumberedElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _numberText;
        [SerializeField] private GameObject _content;
        public INumberedListBody Body { get; private set; }

        public void Construct() => 
            _numberText = GetComponentInChildren<TextMeshProUGUI>();

        public void Set(INumberedListBody body, int number)
        {
            Body = body;

            Vector2 layout = new Vector2(0, 0.5f);
            _numberText.text = number + ".";
            body.RectTransform.SetParent(_content.transform, worldPositionStays: false);
            body.RectTransform.pivot = layout;
            body.RectTransform.anchorMax = layout;
            body.RectTransform.anchorMin = layout;
            body.RectTransform.anchoredPosition = Vector2.zero;
        }
    }
}