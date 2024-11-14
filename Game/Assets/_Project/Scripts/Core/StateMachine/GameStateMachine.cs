using System.Threading;
using _Project.Scripts.Core.Bootstrap;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Gameplay.Battle;

namespace _Project.Scripts.Core.StateMachine
{
    public class GameStateMachine : StateMachineBase, IService
    {
        private GameStateMachine(CancellationToken token) : base(token)
        {
        }

        public override void Update()
        {
            foreach (IStateMachine machine in SubMachines.Values) 
                machine.Update();
        }

        public static GameStateMachine Create(AllServices allServices, CancellationToken token)
        {
            Builder builder = new Builder(token);

            GameStateMachine machine = builder.Machine;
            
            builder
                .DefineSubMachine<BattleStateMachine>()
                .AddState(
                    new BootstrapState(
                        machine,
                        allServices))
                //
                .InitialState<BootstrapState>()
                .Build();

            return machine;
        }

        private class Builder : Builder<GameStateMachine, Builder>
        {
            public Builder(CancellationToken token) =>
                Machine = new GameStateMachine(token);
        }
    }
}