using System;
using System.Threading;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.Gameplay.WinConditions;
using _Project.Scripts.UI;
using _Project.Scripts.UI.History;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class PlayerMovementState : IGameState, IService
    {
        private readonly IPlayersService _playersService;
        private readonly WinPattern _winPattern;
        private readonly MoveHistoryService _moveHistory;
        private readonly GameScreen _gameScreen;
        private readonly BattleStateMachine _stateMachine;
        private CancellationTokenSource _cancellationTokenSource;

        public PlayerMovementState(
            BattleStateMachine stateMachine,
            IPlayersService playersService,
            WinPattern winPattern,
            MoveHistoryService moveHistory,
            GameScreen gameScreen)
        {
            _playersService = playersService;
            _winPattern = winPattern;
            _moveHistory = moveHistory;
            _gameScreen = gameScreen;
            _stateMachine = stateMachine;
        }

        async UniTask IGameState.Enter(StateMachineBase.StateMachineContext context)
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            await ProcessMove(_cancellationTokenSource.Token);
        }

        private async UniTask ProcessMove(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _gameScreen.DisplayMove(_playersService.CurrentPlayer.MoveText);

                int move = await _playersService.PlayerMoveAsync(token);

                if (!_gameScreen.IsPlayLikeButtonHided)
                    _gameScreen.HideSwapSymbolsButton();

                if (move != -1)
                {
                    move--;

                    _playersService.SetMove(move);

                    _moveHistory.Add(new MoveHistoryService.MoveInfo
                    {
                        Position = move,
                        Player = _playersService.CurrentPlayer,
                        Index = _moveHistory.History.Count + 1
                    });

                    bool hasWin = _winPattern.CheckWin(_playersService.PlayerBoard);

                    if (hasWin)
                        _stateMachine.Fire(BattleStateMachine.BattleEvent.END_BATTLE);
                    else if (_playersService.Board.IsFull())
                        _stateMachine.Fire(BattleStateMachine.BattleEvent.END_GAME);
                }
                else
                {
                    throw new Exception("Move is not valid");
                }

                _playersService.SwitchPlayer();
            }
        }

        void IGameState.Exit()
        {
            if (_cancellationTokenSource == null) 
                return;
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}