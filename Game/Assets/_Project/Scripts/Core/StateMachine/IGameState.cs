using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.StateMachine
{
    public interface IGameState
    {
        public UniTask Enter(StateMachineBase.StateMachineContext context);
        public void Exit();
    }
}