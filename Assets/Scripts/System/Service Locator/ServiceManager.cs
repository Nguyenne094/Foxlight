using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace Bap.Service_Locator
{
    public class ServiceManager : MonoBehaviour
    {
        readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        public List<Type> ServiceTypes => services.Keys.ToList();
        public IEnumerable<object> Services => services.Values;

        public bool TryGet<T>(out T service) where T : class{
            Type type = typeof(T);
            if (services.TryGetValue(type, out object obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        public T Get<T>() where T : class {
            Type type = typeof(T);
            if (services.TryGetValue(type, out object service))
            {
                return service as T;
            }

            throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} not registered.");
        }

        public ServiceManager Register<T>(T service)
        {
            Type type = typeof(T);

            if (!services.TryAdd(type, service))
            {
                Debug.LogWarning($"ServiceManager.Register: Service of type {type.FullName} already registered. Overwriting the existing service.");
            }
            
            return this;
        }

        public ServiceManager Register(Type type, object service)
        {
            //Because your services have serveral types, you need to check if the service is of the correct type
            if (!type.IsInstanceOfType(service))
            {
                throw new ArgumentException($"ServiceManager.Register: Service of type {type.FullName} is not of the correct type. Expected {type.FullName}, but got {service.GetType()}.");
            }
            
            if (!services.TryAdd(type, service))
            {
                Debug.LogWarning($"ServiceManager.Register: Service of type {type.FullName} already registered. Overwriting the existing service.");
            }

            return this;
        }
    }
}