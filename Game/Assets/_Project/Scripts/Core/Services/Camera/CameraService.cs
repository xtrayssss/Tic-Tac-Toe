using UnityEngine;

namespace _Project.Scripts.Core.Services.Camera
{
    public class CameraService : IService
    {
        public readonly UnityEngine.Camera MainCamera = UnityEngine.Camera.main;

        public UnityEngine.Camera UICamera { get; } =
            GameObject.FindWithTag("UICamera").GetComponent<UnityEngine.Camera>();
    }
}