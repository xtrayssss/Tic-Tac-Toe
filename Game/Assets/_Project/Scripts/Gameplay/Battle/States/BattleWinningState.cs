using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.Gameplay.WinConditions;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Elements;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class BattleWinningState : IGameState
    {
        private readonly ConfigService _configs;
        private readonly IPlayersService _playersService;
        private readonly GameScreen _gameScreen;
        private readonly WinPattern _winPattern;

        public BattleWinningState(
            ConfigService configs,
            IPlayersService playersService,
            GameScreen gameScreen,
            WinPattern winPattern)
        {
            _configs = configs;
            _playersService = playersService;
            _gameScreen = gameScreen;
            _winPattern = winPattern;
        }

        UniTask IGameState.Enter(StateMachineBase.StateMachineContext context)
        {
            WinLineView winLineView =
                _gameScreen.WinLineView = Object.Instantiate(_configs.WinLinePrefab, _gameScreen.Board);

            winLineView.Construct();

            winLineView.DrawWinLine(_winPattern.GetWineLine(_playersService.Board));

            _gameScreen.DisplayBattleResult(_playersService.CurrentPlayer.WinText);

            return UniTask.CompletedTask;
        }

        void IGameState.Exit()
        {
        }
    }
}