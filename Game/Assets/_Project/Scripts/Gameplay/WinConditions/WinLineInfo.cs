using Unity.Mathematics;

namespace _Project.Scripts.Gameplay.WinConditions
{
    public struct WinLineInfo
    {
        public float2 StartPosition;
        public float2 EndPosition;

        public override string ToString() =>
            $"Start: {StartPosition} End: {EndPosition}";
    }
}