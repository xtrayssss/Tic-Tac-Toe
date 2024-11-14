using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Gameplay.Players;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.UI.History
{
    public class MoveHistoryService : IService
    {
        private readonly List<MoveInfo> _history = new List<MoveInfo>();
        private readonly IPlayersService _playersService;
        private const float REVERT_TO_MOVE_DELAY = 0.2f;

        public IReadOnlyList<MoveInfo> History => _history;

        public event Action<int> OnMoveReverted;
        public event Action<MoveInfo> OnMoveAdded;

        public MoveHistoryService(IPlayersService playersService) =>
            _playersService = playersService;

        public struct MoveInfo
        {
            public int Index;
            public int Position { get; set; }
            public Player Player { get; set; }
        }

        public void Add(MoveInfo moveInfo)
        {
            _history.Add(moveInfo);
            OnMoveAdded?.Invoke(moveInfo);
        }

        public async UniTask RevertToMoveAsync(int moveIndex, CancellationToken token)
        {
            for (; IsValid(moveIndex);) 
                await RevertMoveAsync(token);

            _playersService.CurrentPlayer = _history[moveIndex - 1].Player == _playersService.CurrentPlayer
                ? _playersService.SwitchPlayer()
                : _playersService.CurrentPlayer;
        }

        public bool IsValid(int moveIndex) => 
            _history.Count > moveIndex;

        public async UniTask RevertAll(CancellationToken token)
        {
            for (; _history.Count > 0;)
                await RevertMoveAsync(token);

            _playersService.CurrentPlayer = _playersService.Player1;
        }

        private async UniTask RevertMoveAsync(CancellationToken token)
        {
            int index = _history.Count - 1;

            MoveInfo move = _history[index];

            move.Player.ResetMove(move.Position);

            _history.RemoveAt(index);

            await UniTask.Delay(TimeSpan.FromSeconds(REVERT_TO_MOVE_DELAY), cancellationToken: token);

            OnMoveReverted?.Invoke(index);
        }
    }
}