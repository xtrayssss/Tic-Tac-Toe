using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.TextAnimation
{
    [Serializable]
    [CreateAssetMenu(fileName = "TypewriterAnimation", menuName = "Animations/CreateTypewriterAnimation")]
    public sealed class TypewriterSettings : ScriptableObject
    {
        [SerializeField] private float _baseSpeed = 1f;
        [SerializeField] private float _charactersPerSecond = 10f;
        [SerializeField] private MovementSettings _movement = new MovementSettings();
        [SerializeField] private RotationSettings _rotation = new RotationSettings();
        [SerializeField] private ScaleSettings _scale = new ScaleSettings();
        [SerializeField] private ColorSettings _color = new ColorSettings();

        public float BaseSpeed => _baseSpeed;
        public float CharactersPerSecond => _charactersPerSecond;
        public MovementSettings Movement => _movement;
        public RotationSettings Rotation => _rotation;
        public ScaleSettings Scale => _scale;
        public ColorSettings Color => _color;
    }

    [Serializable]
    public sealed class MovementSettings
    {
        [SerializeField] private bool _isXEnabled;
        [SerializeField] private AnimationSettings _xMovement = new AnimationSettings();
        [SerializeField] private bool _isYEnabled;
        [SerializeField] private AnimationSettings _yMovement = new AnimationSettings();
        [SerializeField] private bool _isZEnabled;
        [SerializeField] private AnimationSettings _zMovement = new AnimationSettings();

        public bool IsXEnabled => _isXEnabled;
        public bool IsYEnabled => _isYEnabled;
        public bool IsZEnabled => _isZEnabled;
        public AnimationSettings XMovement => _xMovement;
        public AnimationSettings YMovement => _yMovement;
        public AnimationSettings ZMovement => _zMovement;
    }

    [Serializable]
    public sealed class RotationSettings
    {
        [SerializeField] private bool _isXEnabled;
        [SerializeField] private AnchoredAnimationSettings _xRotation = new AnchoredAnimationSettings();
        [SerializeField] private bool _isYEnabled;
        [SerializeField] private AnchoredAnimationSettings _yRotation = new AnchoredAnimationSettings();
        [SerializeField] private bool _isZEnabled;
        [SerializeField] private Vector2AnchoredAnimationSettings _zRotation = new Vector2AnchoredAnimationSettings();

        public bool IsXEnabled => _isXEnabled;
        public bool IsYEnabled => _isYEnabled;
        public bool IsZEnabled => _isZEnabled;
        public AnchoredAnimationSettings XRotation => _xRotation;
        public AnchoredAnimationSettings YRotation => _yRotation;
        public Vector2AnchoredAnimationSettings ZRotation => _zRotation;
    }

    [Serializable]
    public sealed class ScaleSettings
    {
        [SerializeField] private bool _isXEnabled;
        [SerializeField] private AnchoredAnimationSettings _xScale = new AnchoredAnimationSettings();
        [SerializeField] private bool _isYEnabled;
        [SerializeField] private AnchoredAnimationSettings _yScale = new AnchoredAnimationSettings();
        [SerializeField] private bool _isZEnabled;
        [SerializeField] private AnchoredAnimationSettings _zScale = new AnchoredAnimationSettings();

        public bool IsXEnabled => _isXEnabled;
        public bool IsYEnabled => _isYEnabled;
        public bool IsZEnabled => _isZEnabled;
        public AnchoredAnimationSettings XScale => _xScale;
        public AnchoredAnimationSettings YScale => _yScale;
        public AnchoredAnimationSettings ZScale => _zScale;
    }

    [Serializable]
    public sealed class ColorSettings
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private GradientAnimationSettings _colorAnimation = new GradientAnimationSettings();

        public bool IsEnabled => _isEnabled;
        public GradientAnimationSettings ColorAnimation => _colorAnimation;
    }

    [Serializable]
    public class AnimationSettings
    {
        [SerializeField] private AnimationCurve _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        [SerializeField] private float _yMultiplier = 1f;
        [SerializeField] private float _speedMultiplier = 1f;

        public AnimationCurve Curve => _curve;
        public float YMultiplier => _yMultiplier;
        public float SpeedMultiplier => _speedMultiplier;
    }

    [Serializable]
    public sealed class Vector2AnchoredAnimationSettings : AnimationSettings
    {
        [SerializeField] private Vector2 _anchor = new Vector2(0.5f, 0.5f);

        public Vector2 Anchor => _anchor;
    }

    [Serializable]
    public sealed class AnchoredAnimationSettings : AnimationSettings
    {
        [SerializeField] private float _anchor = 0.5f;

        public float Anchor => _anchor;
    }

    [Serializable]
    public sealed class GradientAnimationSettings
    {
        [SerializeField] private Gradient _gradient = new Gradient();
        [SerializeField] private float _speedMultiplier = 1f;
        [SerializeField] private bool _useOnlyAlpha;

        public Gradient Gradient => _gradient;
        public float SpeedMultiplier => _speedMultiplier;
        public bool UseOnlyAlpha => _useOnlyAlpha;
    }
}