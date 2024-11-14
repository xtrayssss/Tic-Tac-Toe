using _Project.Scripts.Gameplay.WinConditions;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements
{
    public class WinLineView : MonoBehaviour
    {
        [SerializeField] private Image _lineImage;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _lineWidth = 5f;
        [SerializeField] private float _lineOffset = 0.5f;

        private RectTransform _rectTransform;

        private Sequence _animation;

        public void Construct() =>
            _rectTransform = GetComponent<RectTransform>();

        public void DrawWinLine(WinLineInfo winLine)
        {
            float2 startPos = winLine.StartPosition;
            float2 endPos = winLine.EndPosition;

            float2 direction = endPos - startPos;
            
            float angle = math.degrees(math.atan2(direction.y, direction.x));
            float length = math.length(direction) + _lineOffset * 2;

            float2 centerPos = (startPos + endPos) * 0.5f;

            _rectTransform.sizeDelta = new Vector2(length, _lineWidth);
            _rectTransform.anchoredPosition = centerPos;
            _rectTransform.rotation = Quaternion.Euler(0, 0, angle);

            _lineImage.color = new Color(1, 1, 1, 0);
            transform.localScale = Vector3.one * 0.3f;

            _animation = Sequence.Create()
                .Group(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 1.1f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.OutCubic))
                .Group(
                    Tween.Alpha(
                        target: _lineImage,
                        endValue: 0.5f,
                        duration: _animationDuration * 0.2f))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 0.9f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InOutCubic))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 1.03f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InOutCubic))
                .Group(
                    Tween.Alpha(
                        target: _lineImage,
                        endValue: 0.8f,
                        duration: _animationDuration * 0.1f))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 0.97f,
                        duration: _animationDuration * 0.2f,
                        Ease.InOutCubic))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.OutCubic));
        }

        public ref Sequence Hide()
        {
            _animation.Stop();

            _animation = Sequence.Create()
                .Group(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 0.97f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InCubic))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 1.03f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InOutCubic))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 0.9f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InOutCubic))
                .Group(
                    Tween.Alpha(
                        target: _lineImage,
                        endValue: 0.5f,
                        duration: _animationDuration * 0.1f))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 1.1f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InOutCubic))
                .Chain(
                    Tween.Scale(
                        target: transform,
                        endValue: Vector3.one * 0.3f,
                        duration: _animationDuration * 0.2f,
                        ease: Ease.InCubic))
                .Group(
                    Tween.Alpha(
                        target: _lineImage,
                        endValue: 0f,
                        duration: _animationDuration * 0.2f));

            return ref _animation;
        }
    }
}