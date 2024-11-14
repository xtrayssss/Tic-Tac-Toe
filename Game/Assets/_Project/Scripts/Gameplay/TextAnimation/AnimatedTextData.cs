using TMPro;

namespace _Project.Scripts.Gameplay.TextAnimation
{
    public sealed class AnimatedTextData
    {
        public TMP_Text TextComponent { get; }
        public TMP_TextInfo TextInfo { get; }
        public Typewriter Typewriter { get; private set; }

        public AnimatedTextData(TMP_Text textComponent, TMP_TextInfo textInfo, Typewriter typewriter)
        {
            TextComponent = textComponent;
            TextInfo = textInfo;
            Typewriter = typewriter;
        }

        public void UpdateTypewriter(Typewriter newTypewriter1)
        {
            Typewriter = newTypewriter1;
        }
    }
}