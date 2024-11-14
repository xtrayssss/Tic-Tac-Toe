using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;

namespace _Project.Scripts.Core.Services.Input
{
    public class StandaloneInputService : IInputService
    {
        public event Action<float2> OnTapEvent;
        private UniTaskCompletionSource<float2> _currentTapTask;

        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var position = GetPointerPosition();
                OnTapEvent?.Invoke(position);

                _currentTapTask?.TrySetResult(position);
            }
        }

        private float2 GetPointerPosition() =>
            new float2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);

        public UniTask<float2> WaitForTapAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return UniTask.FromCanceled<float2>(token);

            if (_currentTapTask != null && !_currentTapTask.Task.Status.IsCompleted()) 
                _currentTapTask.TrySetCanceled();

            _currentTapTask = new UniTaskCompletionSource<float2>();
            
            token.Register(() =>
            {
                _currentTapTask?.TrySetCanceled();
                _currentTapTask = null;
            });

            return _currentTapTask.Task;
        }
    }
}