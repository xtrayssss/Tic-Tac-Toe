using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Players
{
    [Serializable]
    public struct SymbolSettings
    {
        [field: SerializeField] public char Symbol { get; set; }
        [field: SerializeField] public Material Design { get; private set; }

        public void Set(in SymbolSettings other)
        {
            Symbol = other.Symbol;
            Design = other.Design;
        }
    }

    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "TicTacToe/" + nameof(PlayerConfig))]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] public PlayerType TypeID;
        [field: SerializeField] public string WinText { get; protected set; }
        [field: SerializeField] public SymbolSettings SymbolSettings { get; protected set; }
        [field: SerializeField] public string MoveText { get; protected set; }
    }
}