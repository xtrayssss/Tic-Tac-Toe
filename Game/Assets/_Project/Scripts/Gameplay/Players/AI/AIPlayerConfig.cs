using UnityEngine;

namespace _Project.Scripts.Gameplay.Players.AI
{
    [CreateAssetMenu(fileName = nameof(AIPlayerConfig), menuName = "TicTacToe/" + nameof(AIPlayerConfig))]
    public class AIPlayerConfig : PlayerConfig
    {
        [field: SerializeField] public bool DebugMode { get; private set; }
        [field: SerializeField] public string ApiKey { get; private set; }
        [field: SerializeField] public string FolderId { get; private set; }
        [field: SerializeField] public string Difficulty { get; private set; }
        [field: SerializeField] public float Timeout { get; private set; }
        [field: SerializeField] public float MoveDelay { get; set; }
    }
}