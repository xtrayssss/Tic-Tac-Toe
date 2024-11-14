using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.StateMachine
{
    public abstract partial class StateMachineBase : IStateMachine
    {
        protected readonly Dictionary<Type, IGameState> States = new Dictionary<Type, IGameState>();
        protected readonly Dictionary<Type, IStateMachine> SubMachines = new Dictionary<Type, IStateMachine>();

        protected IGameState CurrentState;
        protected IGameState PreviousState;

        public readonly StateMachineContext Ctx;

        protected StateMachineBase(CancellationToken token) =>
            Ctx = new StateMachineContext(token);

        public virtual void Update()
        {
        }

        public void GoBack()
        {
            if (PreviousState != null)
            {
                CurrentState?.Exit();
                CurrentState = PreviousState;
                PreviousState = null;
            }
        }

        public void AddSubMachine(IStateMachine machine) =>
            SubMachines[machine.GetType()] = machine;

        private void DefineSubMachine<TSubMachine>(IStateMachine machine) where TSubMachine : IStateMachine =>
            SubMachines.Add(typeof(TSubMachine), machine);

        public TState GetState<TState>() where TState : IGameState =>
            (TState)States[typeof(TState)];

        public void SwitchTo(Type newState)
        {
            CurrentState?.Exit();
            CurrentState = States[newState];
            CurrentState.Enter(Ctx).Forget();
        }

        protected void SwitchToAsync(Type newState)
        {
            CurrentState?.Exit();
            CurrentState = States[newState];
            CurrentState.Enter(Ctx).Forget();
        }

        public TMachine GetSubMachine<TMachine>() =>
            (TMachine)SubMachines[typeof(TMachine)];
    }
}