using _Project.Scripts.UI.Elements.Lists;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements
{
    public class ClearBoardButton : MonoBehaviour, INumberedListBody
    {
        public Button Button { get; private set; }
        public RectTransform RectTransform { get; private set; }

        public ClearBoardButton Construct()
        {
            RectTransform = GetComponent<RectTransform>();
            Button = GetComponent<Button>();

            return this;
        }

        public void SetActive(bool isActive) =>
            Button.interactable = isActive;
    }
}