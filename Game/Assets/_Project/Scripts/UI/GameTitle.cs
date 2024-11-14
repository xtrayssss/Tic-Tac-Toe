using PrimeTween;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GameTitle : MonoBehaviour
    {
        [SerializeField] private float _offsetAboveScreen;
        [SerializeField] private RectTransform _startRectTransform;
        [SerializeField] private Vector2 _startPosition;
        
        private Vector2 _targetPosition;
        private RectTransform _rectTransform;

        public void Construct()
        {
            _rectTransform = GetComponent<RectTransform>();

            _targetPosition = _rectTransform.position;
            _startPosition = _startRectTransform.position;
        }

        public void PlayBounceInDown()
        {
            Tween.PositionY(
                target: transform,
                startValue: _startPosition.y,
                endValue: _targetPosition.y,
                duration: 1f,
                ease: Ease.OutBounce);
        }
    }
}