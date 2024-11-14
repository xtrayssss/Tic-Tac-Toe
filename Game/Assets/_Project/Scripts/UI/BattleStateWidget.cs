using _Project.Scripts.Gameplay.TextAnimation;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class BattleStateWidget : MonoBehaviour
    {
        private Typewriter _typewriter;
        private TextMeshProUGUI _text;

        public void Construct()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _typewriter = GetComponent<Typewriter>();
        }

        public void PlayAnimation()
        {
            _typewriter.CharacterSpeedMultiplier = Mathf.Max(
                1,
                _text.text.Length / _typewriter.TypewriterSettings.CharactersPerSecond
            );

            _typewriter.SpeedMultiplier = Mathf.Max(
                1,
                _typewriter.CharacterSpeedMultiplier * _typewriter.TypewriterSettings.BaseSpeed
            );

            _typewriter.PlayAnimation();
        }

        public void StopAnimation() => 
            _typewriter.StopAnimation();

        public void SetText(string text) =>
            _text.text = text;
    }
}