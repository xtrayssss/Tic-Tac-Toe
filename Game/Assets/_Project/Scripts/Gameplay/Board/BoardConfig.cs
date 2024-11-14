using UnityEngine;

namespace _Project.Scripts.Gameplay.Board
{
    [CreateAssetMenu(fileName = nameof(BoardConfig), menuName = "TicTacToe/" + nameof(BoardConfig))]
    public class BoardConfig : ScriptableObject
    {
        public int Size = 3;
        public float CellSize = 1f;
        public float Offset = 0.1f;
        public CellView CellPrefab;
    }
}