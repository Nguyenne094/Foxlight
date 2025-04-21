using System;
using UnityEngine;
using Utilities;
using Bap.Service_Locator;

namespace Bap.Service_Locator
{
    /// <summary>
    /// This is a simple service locator that uses a singleton pattern at global level.
    /// </summary>
    public class SimpleServiceLocator : Singleton<SimpleServiceLocator>
    {
        ServiceManager _serviceManager = new ServiceManager();

        public SimpleServiceLocator Get<T>(out T service) where T : class
        {
            if (Instance._serviceManager.TryGet(out service))
            {
                return Instance;
            }
                throw new ArgumentException($"Service of type {typeof(T).FullName} not registered.");
        }
        public SimpleServiceLocator Get(Type type, out object service)
        {
            if (Instance._serviceManager.TryGet(out service))
            {
                return Instance;
            }
                throw new ArgumentException($"Service of type {type.FullName} not registered.");
        }
        public SimpleServiceLocator Register<T>(T service)
        {
            Instance._serviceManager.Register(service);
            return Instance;
        }
    }
}