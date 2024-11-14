using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Animations
{
    [RequireComponent(typeof(Button))]
    public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [FormerlySerializedAs("_fill")]
        [FormerlySerializedAs("_background")]
        [Header("Button Elements")]
        [SerializeField]
        protected Image Fill;

        [FormerlySerializedAs("_buttonText")] [SerializeField] protected TextMeshProUGUI ButtonText;

        [SerializeField]  private Color _hoverFillColor = new Color(1f, 0.27f, 0f, 1f);
        [SerializeField] private Color _textHoverColor = new Color(1f, 0.27f, 0f, 1f);
        protected Color TextNormalColor = Color.white;

        [Header("Animation")]
        [SerializeField] private float _animationDuration = 0.3f;
        
        [SerializeField] private float _hoverScaleX = 1.1f;

        private Vector3 _originalScale;
        protected Sequence Animation;
        protected Color FillColor;

        private void Awake() => 
            Setup();

        private void Setup()
        {
            _originalScale = transform.localScale;
            FillColor = Fill.color;
            ButtonText.color = TextNormalColor;
            TextNormalColor = ButtonText.color;
        }

        public virtual void OnPointerEnter(PointerEventData eventData) => 
            AnimateHover(isHovering: true);

        public virtual void OnPointerExit(PointerEventData eventData) => 
            AnimateHover(isHovering: false);

        protected virtual void AnimateHover(bool isHovering)
        {
            Tween fillColorTween = Tween.Color(
                target: Fill,
                endValue: isHovering ? _hoverFillColor : FillColor,
                duration: _animationDuration);

            Tween scaleTween = Tween.ScaleX(
                target: transform,
                endValue: isHovering
                    ? _originalScale.x * _hoverScaleX
                    : _originalScale.x,
                duration: _animationDuration);

            Tween textColorTween = Tween.Color(
                target: ButtonText,
                endValue: isHovering ? _textHoverColor : TextNormalColor,
                duration: _animationDuration);

            Animation = Sequence.Create()
                .Group(fillColorTween)
                .Group(scaleTween)
                .Group(textColorTween);
        }
    }
}