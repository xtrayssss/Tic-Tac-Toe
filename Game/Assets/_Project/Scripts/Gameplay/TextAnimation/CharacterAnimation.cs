namespace _Project.Scripts.Gameplay.TextAnimation
{
    public sealed class CharacterAnimation
    {
        public int Index { get; }
        public float Time { get; set; }

        public CharacterAnimation(int characterIndex)
        {
            Index = characterIndex;
            Time = 0f;
        }
    }
}