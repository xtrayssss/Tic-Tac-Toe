using UnityEngine;

namespace _Project.Scripts.UI.Animations
{
    public static class AnimationCurveExtensions
    {
        public static AnimationCurve AddKeyFluent(this AnimationCurve source, float time, float value)
        {
            source.AddKey(time, value);
            return source;
        }

        public static AnimationCurve AddKeyFluent(this AnimationCurve source, Keyframe keyframe)
        {
            source.AddKey(keyframe);
            return source;
        }
    }
}