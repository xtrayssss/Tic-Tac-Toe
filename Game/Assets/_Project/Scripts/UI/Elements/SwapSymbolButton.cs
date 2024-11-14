using System;
using _Project.Scripts.UI.Animations;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Elements
{
    [Serializable]
    public class SwapSymbolButton : AnimatedButton
    {
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private CanvasGroup _fadable;
        [SerializeField] private CanvasGroup _permanent;
        public Button Button { get; private set; }
        public TextMeshProUGUI Text { get; private set; }

        public void Construct()
        {
            Button = GetComponent<Button>();
            Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void AnimateHover(bool isHovering)
        {
            _permanent.alpha = 1;

            base.AnimateHover(isHovering);
        }

        public void Fade(bool isFadingIn)
        {
            Animation.Stop();

            // fade out
            if (!isFadingIn)
            {
                float fadeOutDuration = _fadeDuration / 2f;

                Tween fillColorTween = Tween.Color(
                    target: Fill,
                    endValue: FillColor,
                    duration: fadeOutDuration,
                    ease: Ease.InCubic);

                Tween textColorTween = Tween.Color(
                    target: ButtonText,
                    endValue: TextNormalColor,
                    duration: fadeOutDuration,
                    ease: Ease.InCubic);

                Tween fadableAlphaTween = Tween.Alpha(
                    target: _fadable,
                    endValue: 0,
                    duration: _fadeDuration,
                    ease: Ease.OutQuad
                );

                Animation = Sequence.Create()
                    .Group(fillColorTween)
                    .Group(textColorTween)
                    .ChainCallback(() => _permanent.alpha = 0)
                    .Insert(
                        atTime: fadeOutDuration * 0.9f,
                        tween: fadableAlphaTween);
            }
            else
            {
                _permanent.alpha = 0;

                Animation = Sequence.Create()
                    .Chain(
                        Tween.Alpha(
                            target: _fadable,
                            endValue: 1,
                            duration: _fadeDuration,
                            ease: Ease.Linear
                        ));
            }
        }

        public void Disable()
        {
            Button.interactable = false;
            Button.enabled = false;
            enabled = false;
        }

        public void Enable()
        {
            Button.interactable = true;
            Button.enabled = true;
            enabled = true;
        }

        public void RemoveListeners() =>
            Button.onClick.RemoveAllListeners();
    }
}