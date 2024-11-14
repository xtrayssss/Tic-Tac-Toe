using System.Threading;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.Services.Camera;
using _Project.Scripts.Core.Services.Config;
using _Project.Scripts.Core.Services.Input;
using _Project.Scripts.Core.StateMachine;
using _Project.Scripts.Gameplay.Battle.States;
using _Project.Scripts.Gameplay.Players;
using _Project.Scripts.Gameplay.WinConditions;
using _Project.Scripts.UI;
using _Project.Scripts.UI.History;

namespace _Project.Scripts.Gameplay.Battle
{
    public partial class BattleStateMachine : StateMachine<BattleStateMachine.BattleEvent>
    {
        private readonly IInputService _inputService;

        private BattleStateMachine(CancellationToken token, IInputService inputService) : base(token) =>
            _inputService = inputService;

        public override void Update() =>
            _inputService.Tick();

        public static BattleStateMachine Create(AllServices allServices, CancellationToken token)
        {
            Builder builder = new Builder(token, allServices.Get<IInputService>());

            BattleStateMachine machine = builder.Machine;

            builder
                .AddState(
                    new BattleCreationState(
                        machine,
                        allServices.Get<IPlayersService>(),
                        allServices.Get<ConfigService>(),
                        allServices.Get<CameraService>(),
                        allServices.Get<MoveHistoryService>(),
                        allServices))
                .AddState(
                    new PlayerMovementState(
                        machine,
                        allServices.Get<IPlayersService>(),
                        allServices.Get<WinPattern>(),
                        allServices.Get<MoveHistoryService>(),
                        allServices.Get<GameScreen>()))
                .AddState(
                    allServices.Register(new MoveRevertingState(
                        machine,
                        allServices.Get<GameScreen>(),
                        allServices.Get<MoveHistoryService>())))
                .AddState(
                    new BattleDrawingState(allServices.Get<GameScreen>()))
                .AddState(
                    new BoardClearingState(
                        machine,
                        allServices.Get<MoveHistoryService>()))
                .AddState(
                    new BattleWinningState(
                        allServices.Get<ConfigService>(),
                        allServices.Get<IPlayersService>(),
                        allServices.Get<GameScreen>(),
                        allServices.Get<WinPattern>()))
                //
                .From<PlayerMovementState>()
                .On(BattleEvent.END_BATTLE)
                .To<BattleWinningState>()
                //
                .From<PlayerMovementState>()
                .On(BattleEvent.REVERT_MOVE)
                .To<MoveRevertingState>()
                //
                .From<PlayerMovementState>()
                .On(BattleEvent.CLEAR_BOARD)
                .To<BoardClearingState>()
                //
                .From<PlayerMovementState>()
                .On(BattleEvent.END_GAME)
                .To<BattleDrawingState>()
                //
                .From<MoveRevertingState>()
                .On(BattleEvent.MAKE_MOVE)
                .To<PlayerMovementState>()
                //
                .From<BoardClearingState>()
                .On(BattleEvent.MAKE_MOVE)
                .To<PlayerMovementState>()
                //
                .From<BattleWinningState>()
                .On(BattleEvent.CLEAR_BOARD)
                .To<BoardClearingState>()
                //
                .From<BattleWinningState>()
                .On(BattleEvent.REVERT_MOVE)
                .To<MoveRevertingState>()
                //
                .From<BattleDrawingState>()
                .On(BattleEvent.CLEAR_BOARD)
                .To<BoardClearingState>()
                //
                .From<BattleDrawingState>()
                .On(BattleEvent.REVERT_MOVE)
                .To<MoveRevertingState>()
                //
                .InitialState<PlayerMovementState>()
                .Build();

            return machine;
        }

        private class Builder : Builder<BattleStateMachine>
        {
            public Builder(CancellationToken token, IInputService inputService) =>
                Machine = new BattleStateMachine(token, inputService);
        }
    }
}