using _Project.Scripts.UI.Animations;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Board
{
    public class CellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _background;
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private float _selectionAlpha;
        [SerializeField] private float _deselectionAlpha;
        [field: SerializeField] public TextMeshProUGUI Symbol { get; set; }
        public RectTransform RectTransform { get; private set; }

        private readonly AnimationCurve _easeOutCurve = new AnimationCurve()
            .AddKeyFluent(new Keyframe(0f, 0f, 2f, 2f))
            .AddKeyFluent(new Keyframe(1f, 1f, 0f, 0f));

        private readonly AnimationCurve _easeInCurve = new AnimationCurve()
            .AddKeyFluent(new Keyframe(0f, 0f, 0f, 0f))
            .AddKeyFluent(new Keyframe(1f, 1f, 2f, 2f));

        private Image _image;
        private Tween _selectionTween;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _image = GetComponentInChildren<Image>();
        }

        public void SetSymbol(char symbol, Material material)
        {
            Symbol.text = symbol.ToString();
            Symbol.fontMaterial = material;
        }

        public void SetSymbol(char symbol) =>
            Symbol.text = symbol.ToString();

        [Button]
        public void PlayFlipInX()
        {
            Sequence rotation = Sequence.Create()
                .Chain(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            startValue = new Vector3(90, 0, 0),
                            endValue = new Vector3(-20f, 0f, 0f),
                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.4f,
                                customEase = _easeInCurve
                            }
                        }))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        endValue: new Vector3(10f, 0f, 0f),
                        duration: _animationDuration * 0.2f))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        new Vector3(-5f, 0f, 0f),
                        _animationDuration * 0.2f))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        Vector3.zero,
                        _animationDuration * 0.2f));

            Sequence.Create()
                .Group(
                    Tween.Alpha(
                        _image,
                        startValue: 0,
                        endValue: _image.color.a,
                        duration: _animationDuration * 0.6f))
                .Group(rotation);
        }

        [Button]
        public void PlayFlipInY()
        {
            Sequence rotation = Sequence.Create()
                .Chain(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            startValue = new Vector3(0, 90, 0),
                            endValue = new Vector3(0, -20f, 0f),
                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.4f,
                                customEase = _easeInCurve
                            }
                        }))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        endValue: new Vector3(0f, 10f, 0f),
                        duration: _animationDuration * 0.2f))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        new Vector3(0f, -5f, 0f),
                        _animationDuration * 0.2f))
                .Chain(
                    Tween.LocalRotation(
                        transform,
                        Vector3.zero,
                        _animationDuration * 0.2f));

            Tween alpha = Tween.Alpha(
                _image,
                startValue: 0,
                endValue: 1,
                duration: _animationDuration * 0.6f);

            Sequence.Create()
                .Group(alpha)
                .Group(rotation);
        }

        [Button]
        public void PlayFlipOutX()
        {
            Tween alpha = Tween.Alpha(
                _image,
                settings: new TweenSettings<float>
                {
                    startValue = 1,
                    endValue = 0,
                    settings = new TweenSettings
                    {
                        duration = _animationDuration,
                        customEase = _easeInCurve
                    }
                }
            );

            Sequence.Create()
                .Chain(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            startValue = Vector3.zero,
                            endValue = new Vector3(20f, 0f, 0f),
                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.3f,
                                customEase = _easeInCurve
                            }
                        }))
                .Group(alpha)
                .Group(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            endValue = new Vector3(90f, 0f, 0f),

                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.7f,
                                customEase = _easeOutCurve
                            }
                        }));
        }

        [Button]
        public void PlayFlipOutY()
        {
            Tween alpha = Tween.Alpha(
                _image,
                settings: new TweenSettings<float>
                {
                    startValue = 1,
                    endValue = 0,
                    settings = new TweenSettings
                    {
                        duration = _animationDuration,
                        customEase = _easeInCurve
                    }
                }
            );

            Sequence.Create()
                .Chain(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            startValue = Vector3.zero,
                            endValue = new Vector3(0f, 20f, 0f),
                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.3f,
                                customEase = _easeInCurve
                            }
                        }))
                .Group(alpha)
                .Group(
                    Tween.LocalRotation(
                        target: transform,
                        localEulerAnglesSettings: new TweenSettings<Vector3>
                        {
                            endValue = new Vector3(0f, 90f, 0f),

                            settings = new TweenSettings
                            {
                                duration = _animationDuration * 0.7f,
                                customEase = _easeOutCurve
                            }
                        }));
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_selectionTween.isAlive)
                _selectionTween.Stop();

            _selectionTween = Tween.Alpha(
                _background,
                endValue: _selectionAlpha,
                duration: 0.05f);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_selectionTween.isAlive)
                _selectionTween.Stop();

            _selectionTween = Tween.Alpha(
                _background,
                endValue: _deselectionAlpha,
                duration: 0.05f);
        }
    }
}