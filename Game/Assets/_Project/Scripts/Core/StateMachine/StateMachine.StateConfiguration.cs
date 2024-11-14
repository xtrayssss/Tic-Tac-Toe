using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.StateMachine
{
    public partial class StateMachine<TEvent>
    {
        public class StateConfiguration<TMachine> where TMachine : StateMachine<TEvent>
        {
            private readonly Builder<TMachine> _builder;
            private TEvent _lastEvent;
            private readonly Type _stateType;
            private readonly Dictionary<TEvent, Type> _transitions;

            public StateConfiguration(
                Builder<TMachine> builder,
                Type stateType,
                Dictionary<TEvent, Type> transitions)
            {
                _builder = builder;
                _stateType = stateType;
                _transitions = transitions;
            }

            public StateConfiguration<TMachine> On(TEvent eventType)
            {
                _transitions.Add(eventType, null);
                _lastEvent = eventType;
                return this;
            }

            public StateConfiguration<TMachine> To<TState>() where TState : IGameState
            {
                _transitions[_lastEvent] = typeof(TState);
                _builder.Machine._transitions[_stateType] = _transitions;
                return this;
            }

            public Builder<TMachine> InitialState<TState>() where TState : IGameState =>
                _builder.InitialState<TState>();

            public StateConfiguration<TMachine> From<TState>() where TState : IGameState =>
                _builder.From<TState>();
        }
    }
}