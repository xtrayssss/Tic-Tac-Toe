using _Project.Scripts.UI.Elements.Lists;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements
{
    public class RevertMoveButton : MonoBehaviour, INumberedListBody
    {
        [SerializeField] private TextMeshProUGUI _text;
        public RectTransform RectTransform { get; private set; }
        public Button Button { get; private set; }

        public RevertMoveButton Construct()
        {
            RectTransform = GetComponent<RectTransform>();
            Button = GetComponent<Button>();

            return this;
        }

        public void SetActive(bool isActive) =>
            Button.interactable = isActive;

        public RevertMoveButton SetText(string text)
        {
            _text.text = text;
            return this;
        }

        public RevertMoveButton AddListener(UnityAction action)
        {
            Button.onClick.AddListener(action);
            return this;
        }
    }
}