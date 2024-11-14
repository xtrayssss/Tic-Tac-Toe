using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Services
{
    
    public class AllServices
    {
        private static AllServices _instance;
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static AllServices Instance => _instance ??= new AllServices();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() => 
            _instance?.Clear();

        public TService Register<TService>(TService service) where TService : IService
        {
            Type type = typeof(TService);
            _services.Add(type, service);
            return service;
        }

        public T Get<T>() where T : class, IService
        {
            Type type = typeof(T);

            if (!_services.TryGetValue(type, out IService service))
            {
                Debug.LogError($"Service of type {type} is not registered!");

                return null;
            }

            return service as T;
        }

        public void Clear() =>
            _services.Clear();
    }
}