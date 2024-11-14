using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class BattleDrawingState : IGameState
    {
        private readonly GameScreen _gameScreen;

        public BattleDrawingState(GameScreen gameScreen) => 
            _gameScreen = gameScreen;

        UniTask IGameState.Enter(StateMachineBase.StateMachineContext context)
        {
            _gameScreen.DisplayBattleResult("Draw!!!");

            return UniTask.CompletedTask;
        }

        void IGameState.Exit()
        {
        }
    }
}