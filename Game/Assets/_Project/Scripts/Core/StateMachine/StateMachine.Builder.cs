using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.StateMachine
{
    public abstract partial class StateMachine<TEvent>
    {
        public class Builder<TMachine> : Builder<TMachine, Builder<TMachine>> where TMachine : StateMachine<TEvent>
        {
            public StateConfiguration<TMachine> From<TState>() where TState : IGameState
            {
                Machine._transitions.TryGetValue(typeof(TState), out Dictionary<TEvent, Type> transitions);
                transitions ??= new Dictionary<TEvent, Type>();
                return new StateConfiguration<TMachine>(this, typeof(TState), transitions);
            }
        }
    }
}