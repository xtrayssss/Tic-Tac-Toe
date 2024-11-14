using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.Services.Camera;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.Gameplay.Board;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.UI;
using _Project.Scripts.UI.History;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Battle.States
{
    public class BattleCreationState : IGameState
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly IPlayersService _playersService;
        private readonly ConfigService _configService;
        private readonly CameraService _cameraService;
        private readonly MoveHistoryService _moveHistoryService;

        public BattleCreationState(
            BattleStateMachine stateMachine,
            IPlayersService playersService,
            ConfigService configService,
            CameraService cameraService,
            MoveHistoryService moveHistoryService,
            AllServices allServices)
        {
            _stateMachine = stateMachine;
            _playersService = playersService;
            _configService = configService;
            _cameraService = cameraService;
            _moveHistoryService = moveHistoryService;

            BoardGenerator boardGenerator = new BoardGenerator(_configService.Board);
            CreatePlayers(_playersService, boardGenerator);

            GameScreen gameScreen = allServices.Register(CreateGameScreen());

            boardGenerator.Generate(gameScreen.Board);
        }

        UniTask IGameState.Enter(StateMachineBase.StateMachineContext context) => 
            UniTask.CompletedTask;

        void IGameState.Exit()
        {
        }

        private GameScreen CreateGameScreen()
        {
            GameScreen prefab = _configService.GameScreenPrefab;
            
            GameScreen screen = Object.Instantiate(prefab)
                .Construct(
                    _cameraService.UICamera,
                    _moveHistoryService,
                    _stateMachine,
                    _playersService,
                    _configService)
                .Initialize();

            screen.PlayTitleBounceInDown();
            screen.enabled = true;
            return screen;
        }

        private static void CreatePlayers(IPlayersService playersService, BoardGenerator boardGenerator)
        {
            Player player1 = playersService.CreatePlayer(PlayerType.HUMAN, boardGenerator);
            Player player2 = playersService.CreatePlayer(PlayerType.RANDOM, boardGenerator);

            playersService.Player1 = player1;
            playersService.Player2 = player2;

            playersService.SwitchPlayer();
        }
    }
}