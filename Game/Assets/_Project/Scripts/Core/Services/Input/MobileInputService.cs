using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Core.Services.Input
{
    public class MobileInputService : IInputService
    {
        public event Action<float2> OnTapEvent;
        private Vector2 _dragStartPosition;
        private const float TAP_THRESHOLD = 10f;
        private UniTaskCompletionSource<float2> _currentTapTask;

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

        public void Tick()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _dragStartPosition = touch.position;
                        break;
                    case TouchPhase.Ended:
                        Vector2 deltaPosition = touch.position - _dragStartPosition;

                        if (deltaPosition.magnitude < TAP_THRESHOLD)
                        {
                            float2 position = new float2(touch.position.x, touch.position.y);
                            OnTapEvent?.Invoke(position);
                            
                            _currentTapTask?.TrySetResult(position);
                        }
                        break;
                }
            }
        }
    }
}