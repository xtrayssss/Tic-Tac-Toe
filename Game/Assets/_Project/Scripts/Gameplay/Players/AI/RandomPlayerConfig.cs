using UnityEngine;

namespace _Project.Scripts.Gameplay.Players.AI
{
    [CreateAssetMenu(fileName = nameof(RandomPlayerConfig), menuName = "TicTacToe/" + nameof(RandomPlayerConfig))]
    public class RandomPlayerConfig : PlayerConfig
    {
        [field: SerializeField] public float MoveDelay { get; protected set; }
    }
}