using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Gameplay.Board;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Players.AI
{
    public class RandomPlayer : Player
    {
        private readonly float _moveDelay;

        public RandomPlayer(RandomPlayerConfig config, BoardGenerator boardGenerator) : base(config, boardGenerator) => 
            _moveDelay = config.MoveDelay;

        public override async UniTask<int> GetMoveAsync(CancellationToken token)
        {
            List<int> freeCells = BoardGenerator.Board.GetFreeCells();

            if (freeCells.Count == 0)
                return -1;
            
            int randomIndex = Random.Range(0, freeCells.Count);

            await UniTask.Delay(TimeSpan.FromSeconds(_moveDelay), cancellationToken: token, cancelImmediately: true);

            return freeCells[randomIndex] + 1;
        }
    }
}