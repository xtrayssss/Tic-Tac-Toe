using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.Services.Camera;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Core.Services.Input;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.Gameplay.Battle;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.Gameplay.WinConditions;
using _Project.Scripts.UI.History;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Bootstrap
{
    public class BootstrapState : IGameState
    {
        public BootstrapState(GameStateMachine stateMachine, AllServices allServices)
        {
            var stateMachine1 = stateMachine;
            var services1 = allServices;

            services1.Register(stateMachine1);

            IInputService inputService = services1.Register(CreateInputService());

            ConfigService configService = services1.Register(new ConfigService());
            configService.Initialize();

            services1.Register(new WinPattern(configService.Board.Size));
            services1.Register(new CameraService());

            IPlayersService playersService = services1.Register<IPlayersService>(
                new PlayersService(
                    inputService,
                    configService));

            services1.Register(new MoveHistoryService(playersService));

            BattleStateMachine battleStateMachine = BattleStateMachine.Create(services1, stateMachine1.Ctx.Token);
            stateMachine1.AddSubMachine(battleStateMachine);
        }

        UniTask IGameState.Enter(StateMachineBase.StateMachineContext context) => 
            UniTask.CompletedTask;

        private static IInputService CreateInputService()
        {
            IInputService inputService;

#if UNITY_ANDROID || UNITY_IOS
            inputService = new MobileInputService();
#else
            inputService = new StandaloneInputService();
#endif
            return inputService;
        }

        public void Exit()
        {
        }
    }
}