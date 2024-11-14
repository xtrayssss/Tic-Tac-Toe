using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.TextAnimation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class Typewriter : MonoBehaviour
    {
        private readonly List<CharacterAnimation> _characterAnimations = new List<CharacterAnimation>();
        private TMP_Text _textComponent;
        private TMP_TextInfo _textInfo;
        private Coroutine _animationCoroutine;
        private int _previousTextLength;

        [SerializeField] private TypewriterSettings _typewriterSettings;
        [SerializeField] private bool _startOnEnable;
        [SerializeField] private bool _startOnStart;
        [field: SerializeField] public float SpeedMultiplier { get; set; } = 1.0f;
        [field: SerializeField] public float CharacterSpeedMultiplier { get; set; } = 1.0f;
        [SerializeField] private float _skipSpeedMultiplier = 4.0f;
        [SerializeField] private int _skipCharacterCount = 4;

        public TypewriterSettings TypewriterSettings => _typewriterSettings;
        public event Action<int> OnCharacterAppeared;
        public event Action OnTypewriterCompleted;
        public UnityEvent OnAnimationEnd;

        public bool IsPaused { get; set; }
        public bool IsSkipping { get; private set; }
        public bool IsStopped;

        private void OnEnable()
        {
            InitializeComponents();
            if (!Application.isPlaying) return;

            HideText();
            if (_startOnEnable) PlayAnimation();
        }

        private void Start()
        {
            if (!Application.isPlaying) return;

            HideText();
            if (_startOnStart) PlayAnimation();
        }

        private void InitializeComponents()
        {
            _textComponent = GetComponent<TMP_Text>();
            _textInfo = _textComponent.textInfo;
            TextAnimatorManager.AddTypewriter(this, _textComponent, _textInfo);
        }

        public void PlayAnimation()
        {
            IsStopped = false;

            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimateText());
        }

        public void StopAnimation()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _textComponent.maxVisibleCharacters = int.MaxValue;
            _characterAnimations.Clear();
            IsStopped = true;
        }

        public void PauseAnimation()
        {
            IsPaused = true;
        }

        public void ResumeAnimation()
        {
            IsPaused = false;
        }

        public void SkipAnimation()
        {
            IsSkipping = true;
        }

        public void HideText()
        {
            _textComponent.maxVisibleCharacters = 0;
        }

        private IEnumerator AnimateText()
        {
            if (!Application.isPlaying) yield break;

            PrepareAnimation();

            while (_textInfo.characterCount == 0)
            {
                yield return null;
            }

            yield return ProcessCharacters();

            CompleteAnimation();
        }

        private void PrepareAnimation()
        {
            IsPaused = false;
            _characterAnimations.Clear();
        }

        private IEnumerator ProcessCharacters()
        {
            int currentCharacter = 0;

            while (currentCharacter < _textInfo.characterCount)
            {
                if (IsPaused)
                {
                    yield return null;
                    continue;
                }

                int charsToProcess = IsSkipping ? _skipCharacterCount : 1;
                for (int i = 0; i < charsToProcess && currentCharacter < _textInfo.characterCount; i++)
                {
                    ProcessCharacter(currentCharacter);
                    currentCharacter++;
                }

                float characterDelay = CalculateCharacterDelay();
                if (characterDelay > 0)
                {
                    yield return new WaitForSecondsRealtime(characterDelay);
                }
            }
        }

        private void ProcessCharacter(int index)
        {
            _characterAnimations.Add(new CharacterAnimation(index));
            _textComponent.maxVisibleCharacters = index + 1;
            TextAnimatorManager.ProcessAnimationsWithoutTime();
            OnCharacterAppeared?.Invoke(index);
        }

        private float CalculateCharacterDelay()
        {
            float baseDelay = _typewriterSettings.CharactersPerSecond * CharacterSpeedMultiplier;
            float speedMultiplier = IsSkipping ? _skipSpeedMultiplier : 1f;
            return baseDelay > 0 ? 1f / (baseDelay * speedMultiplier) : 0f;
        }

        private void CompleteAnimation()
        {
            IsSkipping = false;
            OnAnimationEnd?.Invoke();
            OnTypewriterCompleted?.Invoke();
        }

        public void ProcessAnimations(bool updateTime = true)
        {
            foreach (CharacterAnimation charAnim in _characterAnimations)
            {
                ProcessCharacterAnimation(charAnim, updateTime);
            }
        }

        private void ProcessCharacterAnimation(CharacterAnimation charAnim, bool updateTime)
        {
            TMP_CharacterInfo charInfo = _textInfo.characterInfo[charAnim.Index];
            if (!charInfo.isVisible) return;

            int materialIndex = charInfo.materialReferenceIndex;
            Vector3[] vertexPositions = _textInfo.meshInfo[materialIndex].vertices;
            Color32[] vertexColors = _textInfo.meshInfo[materialIndex].colors32;

            ProcessMovement(charInfo, vertexPositions, charAnim.Time);
            ProcessRotation(charInfo, vertexPositions, charAnim.Time);
            ProcessScale(charInfo, vertexPositions, charAnim.Time);
            ProcessColor(charInfo, vertexColors, charAnim.Time);

            if (updateTime)
            {
                float deltaTime = Time.deltaTime * _typewriterSettings.BaseSpeed * SpeedMultiplier;
                charAnim.Time += deltaTime * (IsSkipping ? _skipSpeedMultiplier : 1f);
            }
        }

        private void ProcessMovement(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            if (_typewriterSettings.Movement.IsXEnabled)
                ApplyMovementX(charInfo, vertices, time);
            if (_typewriterSettings.Movement.IsYEnabled)
                ApplyMovementY(charInfo, vertices, time);
            if (_typewriterSettings.Movement.IsZEnabled)
                ApplyMovementZ(charInfo, vertices, time);
        }

        private void ApplyMovementX(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var moveX = _typewriterSettings.Movement.XMovement;
            float currentValue = CalculateAnimationValue(time, moveX.SpeedMultiplier, moveX.Curve, moveX.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            float size = max.x - min.x;

            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i].x += currentValue * size;
            }
        }

        private void ApplyMovementY(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var moveY = _typewriterSettings.Movement.YMovement;
            float currentValue = CalculateAnimationValue(time, moveY.SpeedMultiplier, moveY.Curve, moveY.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            float size = max.y - min.y;

            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i].y += currentValue * size;
            }
        }

        private void ApplyMovementZ(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var moveZ = _typewriterSettings.Movement.ZMovement;
            float currentValue = CalculateAnimationValue(time, moveZ.SpeedMultiplier, moveZ.Curve, moveZ.YMultiplier);

            for (int i = 0; i < 4; i++)
            {
                vertices[charInfo.vertexIndex + i].z += currentValue;
            }
        }

        private void ProcessRotation(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            if (_typewriterSettings.Rotation.IsXEnabled)
                ApplyRotationX(charInfo, vertices, time);
            if (_typewriterSettings.Rotation.IsYEnabled)
                ApplyRotationY(charInfo, vertices, time);
            if (_typewriterSettings.Rotation.IsZEnabled)
                ApplyRotationZ(charInfo, vertices, time);
        }

        private void ApplyRotationX(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var rotX = _typewriterSettings.Rotation.XRotation;
            float currentValue = CalculateAnimationValue(time, rotX.SpeedMultiplier, rotX.Curve, rotX.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            Vector3 pivot = new Vector3(0, Mathf.Lerp(min.y, max.y, rotX.Anchor), 0);

            ApplyRotationToVertices(charInfo, vertices, Quaternion.Euler(currentValue, 0, 0), pivot);
        }

        private void ApplyRotationY(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var rotY = _typewriterSettings.Rotation.YRotation;
            float currentValue = CalculateAnimationValue(time, rotY.SpeedMultiplier, rotY.Curve, rotY.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            Vector3 pivot = new Vector3(Mathf.Lerp(min.x, max.x, rotY.Anchor), 0, 0);

            ApplyRotationToVertices(charInfo, vertices, Quaternion.Euler(0, currentValue, 0), pivot);
        }

        private void ApplyRotationZ(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var rotZ = _typewriterSettings.Rotation.ZRotation;
            float currentValue = CalculateAnimationValue(time, rotZ.SpeedMultiplier, rotZ.Curve, rotZ.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            Vector3 pivot = new Vector3(
                Mathf.Lerp(min.x, max.x, rotZ.Anchor.x),
                Mathf.Lerp(min.y, max.y, rotZ.Anchor.y),
                0
            );

            ApplyRotationToVertices(charInfo, vertices, Quaternion.Euler(0, 0, currentValue), pivot);
        }

        private void ApplyRotationToVertices(TMP_CharacterInfo charInfo, Vector3[] vertices, Quaternion rotation,
            Vector3 pivot)
        {
            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                vertices[vertexIndex] -= pivot;
                vertices[vertexIndex] = rotation * vertices[vertexIndex];
                vertices[vertexIndex] += pivot;
            }
        }

        private void ProcessScale(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            if (_typewriterSettings.Scale.IsXEnabled)
                ApplyScaleX(charInfo, vertices, time);
            if (_typewriterSettings.Scale.IsYEnabled)
                ApplyScaleY(charInfo, vertices, time);
            if (_typewriterSettings.Scale.IsZEnabled)
                ApplyScaleZ(charInfo, vertices, time);
        }

        private void ApplyScaleX(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var scaleX = _typewriterSettings.Scale.XScale;
            float currentValue =
                CalculateAnimationValue(time, scaleX.SpeedMultiplier, scaleX.Curve, scaleX.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            float pivot = Mathf.Lerp(min.x, max.x, scaleX.Anchor);

            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                vertices[vertexIndex].x = (vertices[vertexIndex].x - pivot) * currentValue + pivot;
            }
        }

        private void ApplyScaleY(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var scaleY = _typewriterSettings.Scale.YScale;
            float currentValue =
                CalculateAnimationValue(time, scaleY.SpeedMultiplier, scaleY.Curve, scaleY.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            float pivot = Mathf.Lerp(min.y, max.y, scaleY.Anchor);

            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                vertices[vertexIndex].y = (vertices[vertexIndex].y - pivot) * currentValue + pivot;
            }
        }

        private void ApplyScaleZ(TMP_CharacterInfo charInfo, Vector3[] vertices, float time)
        {
            var scaleZ = _typewriterSettings.Scale.ZScale;
            float currentValue =
                CalculateAnimationValue(time, scaleZ.SpeedMultiplier, scaleZ.Curve, scaleZ.YMultiplier);

            Vector3 min = vertices[charInfo.vertexIndex];
            Vector3 max = vertices[charInfo.vertexIndex + 2];
            float pivot = Mathf.Lerp(min.z, max.z, scaleZ.Anchor);

            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                vertices[vertexIndex].z = (vertices[vertexIndex].z - pivot) * currentValue + pivot;
            }
        }

        private void ProcessColor(TMP_CharacterInfo charInfo, Color32[] colors, float time)
        {
            if (!_typewriterSettings.Color.IsEnabled) return;

            var colorSettings = _typewriterSettings.Color;
            float curveTime = Mathf.Clamp01(time * colorSettings.ColorAnimation.SpeedMultiplier);
            Color currentColor = colorSettings.ColorAnimation.Gradient.Evaluate(curveTime);

            for (int i = 0; i < 4; i++)
            {
                int vertexIndex = charInfo.vertexIndex + i;
                colors[vertexIndex] = colorSettings.ColorAnimation.UseOnlyAlpha
                    ? new Color32(
                        colors[vertexIndex].r,
                        colors[vertexIndex].g,
                        colors[vertexIndex].b,
                        (byte)(currentColor.a * 255))
                    : currentColor;
            }
        }

        private float CalculateAnimationValue(float time, float speedMultiplier, AnimationCurve curve,
            float valueMultiplier)
        {
            float curveTime = Mathf.Clamp01(time * speedMultiplier);
            return curve.Evaluate(curveTime) * valueMultiplier;
        }
    }
}