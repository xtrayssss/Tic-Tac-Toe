using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements.ScrollSnap
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollSnapFullView : ScrollSnap
    {
        [FormerlySerializedAs("itemSpacing")]
        [Header("Full View Settings")]
        [SerializeField] private float _itemSpacing = 10f;

        private RectTransform _viewport;
        private Vector2 _lastContentSize;

        protected override void Start()
        {
            base.Start();
            _viewport = ScrollRect.viewport;
            UpdateSnapPositions();
        }

        protected override void Update()
        {
            base.Update();
            
            UpdateSnapPositions();
        }

        private void UpdateSnapPositions()
        {
            if (Elements.Count <= 1)
                return;

            float totalHeight = Content.sizeDelta.y - _viewport.rect.height;

            if (totalHeight <= 0)
                return;

            Debug.Log("Change");
            for (int i = 0; i < Elements.Count; i++)
            {
                ScrollSnapElement element = Elements[i];

                float itemHeight = element.RectTransform.rect.height;
                float itemCenter = itemHeight / 2 + i * (itemHeight + _itemSpacing);
                element.Position = Mathf.Clamp01(itemCenter / totalHeight);
            }
        }
    }
}