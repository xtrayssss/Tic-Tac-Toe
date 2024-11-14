using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Core.StateMachine
{
    public abstract partial class StateMachine<TEvent> : StateMachineBase where TEvent : IComparable
    {
        private readonly Dictionary<Type, Dictionary<TEvent, Type>> _transitions =
            new Dictionary<Type, Dictionary<TEvent, Type>>();

        protected StateMachine(CancellationToken token) : base(token)
        {
        }

        public void Fire(TEvent eventType, StateMachineContext stateMachineContext = null)
        {
            if (CurrentState != null)
            {
                if (!_transitions.TryGetValue(CurrentState.GetType(), out Dictionary<TEvent, Type> transitions))
                {
                    Debug.LogError($"Failed to get current state: {CurrentState}");

                    return;
                }

                if (!transitions.TryGetValue(eventType, out Type nextStateType))
                {
                    Debug.LogWarning($"No transition found for trigger '{eventType}' in state '{CurrentState}'");

                    return;
                }

                IGameState nextState = States[nextStateType];

                CurrentState?.Exit();
                PreviousState = CurrentState;
                CurrentState = nextState;
                CurrentState.Enter(stateMachineContext ?? Ctx).Forget();
            }
        }
    }
}