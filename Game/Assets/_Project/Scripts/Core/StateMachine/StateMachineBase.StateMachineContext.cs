using System.Threading;

namespace _Project.Scripts.Core.StateMachine
{
    public abstract partial class StateMachineBase
    {
        public class StateMachineContext
        {
            public CancellationToken Token { get; set; }

            public StateMachineContext(CancellationToken token) => 
                Token = token;
        }
    }
}