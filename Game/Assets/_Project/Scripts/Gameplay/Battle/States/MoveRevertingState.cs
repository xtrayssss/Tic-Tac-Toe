using System.Threading;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.UI;
using _Project.Scripts.UI.History;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class MoveRevertingState : IGameState, IService
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly GameScreen _gameScreen;
        private readonly MoveHistoryService _moveHistoryService;

        public MoveRevertingState(
            BattleStateMachine stateMachine,
            GameScreen gameScreen,
            MoveHistoryService moveHistoryService)
        {
            _stateMachine = stateMachine;
            _gameScreen = gameScreen;
            _moveHistoryService = moveHistoryService;
        }

        async UniTask IGameState.Enter(StateMachineBase.StateMachineContext context)
        {
            if (context is not Context revertMoveContext)
                return;

            if (_moveHistoryService.IsValid(revertMoveContext.MoveIndex))
            {
                if (_gameScreen.WinLineView != null)
                {
                    _gameScreen.WinLineView
                        .Hide()
                        .ChainCallback(
                            target: _gameScreen.WinLineView,
                            line => Object.Destroy(line.gameObject));
                }

                await _moveHistoryService.RevertToMoveAsync(revertMoveContext.MoveIndex, context.Token);

                _stateMachine.Fire(BattleStateMachine.BattleEvent.MAKE_MOVE);
            }
            else
            {
                _stateMachine.GoBack();
            }
        }

        void IGameState.Exit()
        {
        }

        public class Context : StateMachineBase.StateMachineContext
        {
            public int MoveIndex { get; }

            public Context(CancellationToken token, int moveIndex) : base(token) =>
                MoveIndex = moveIndex;
        }
    }
}