using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;

namespace _Project.Scripts.Core.Services.Input
{
    public interface IInputService : IService
    {
        public event Action<float2> OnTapEvent;
        public void Tick();
        public UniTask<float2> WaitForTapAsync(CancellationToken token);
    }
}