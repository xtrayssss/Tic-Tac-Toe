using _Project.Scripts.Core.Services;
using _Project.Scripts.Core.StateMachine;
using UnityEngine;

namespace _Project.Scripts.Core.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        public void Start() => 
            _gameStateMachine = GameStateMachine.Create(AllServices.Instance, destroyCancellationToken);

        private void Update() => 
            _gameStateMachine.Update();
    }
}