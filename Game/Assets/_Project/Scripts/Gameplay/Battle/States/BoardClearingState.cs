using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.UI;
using _Project.Scripts.UI.History;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class BoardClearingState : IGameState
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly MoveHistoryService _moveHistoryService;

        public BoardClearingState(
            BattleStateMachine stateMachine,
            MoveHistoryService moveHistoryService)
        {
            _stateMachine = stateMachine;
            _moveHistoryService = moveHistoryService;
        }

        async UniTask IGameState.Enter(StateMachineBase.StateMachineContext context)
        {
            GameScreen gameScreen = AllServices.Instance.Get<GameScreen>();

            if (_moveHistoryService.History.Count > 0)
            {
                if (gameScreen.WinLineView != null)
                    gameScreen.WinLineView
                        .Hide()
                        .ChainCallback(
                            target: gameScreen.WinLineView,
                            line => Object.Destroy(line.gameObject));

                await _moveHistoryService.RevertAll(context.Token);
            }

            gameScreen.DisplayPlayLikeButton();
            _stateMachine.Fire(BattleStateMachine.BattleEvent.MAKE_MOVE);
        }

        void IGameState.Exit()
        {
        }
    }
}