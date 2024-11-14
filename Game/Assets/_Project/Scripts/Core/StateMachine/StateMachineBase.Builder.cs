namespace _Project.Scripts.Core.StateMachine
{
    public abstract partial class StateMachineBase
    {
        public class Builder<TMachine, TBuilder> where TMachine : StateMachineBase where TBuilder : Builder<TMachine, TBuilder>
        {
            public TMachine Machine;

            public TBuilder AddState(IGameState state)
            {
                Machine.States.Add(state.GetType(), state);
                return (TBuilder)this;
            }

            public TBuilder InitialState<TState>() where TState : IGameState
            {
                Machine.SwitchTo(typeof(TState));
                return (TBuilder)this;
            }

            public TBuilder DefineSubMachine<TSubMachine>() where TSubMachine : IStateMachine
            {
                Machine.DefineSubMachine<TSubMachine>(null);

                return (TBuilder)this;
            }

            public TMachine Build() =>
                Machine;
        }
    }
}