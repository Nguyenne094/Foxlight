using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bap.EventChannel;
using Bap.Manager;
using UnityEngine;

namespace Bap.DependencyInjection {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : PropertyAttribute { }
    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute { }

    public class Injector : Singleton<Injector> {
        [SerializeField] private VoidEventChannelSO _onSceneGroupLoaded;
        
        const BindingFlags k_bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

        public override void Awake()
        {
            base.Awake();
            _onSceneGroupLoaded.OnEventRaised += StartInjection;
        }
        
        private void OnDestroy()
        {
            _onSceneGroupLoaded.OnEventRaised -= StartInjection;
        }

        private void StartInjection()
        {
            Debug.Log("[DI] Start injection");
            var monoBehaviours = GetAllMonoBehaviours();

            foreach (var b in monoBehaviours)
            {
                Provides(b);
            }
            foreach (var b in monoBehaviours)
            {
                InjectsFields(b);
            }
            foreach (var b in monoBehaviours)
            {
                InjectsMethods(b);
            }
        }

        private void Provides(MonoBehaviour behaviour)
        {
            var methods = behaviour.GetType().GetMethods(k_bindingFlags).
                    Where(m => Attribute.IsDefined(m, typeof(ProvideAttribute)));

            foreach (var method in methods)
            {
                var returnType = method.ReturnType;
                var instance = method.Invoke(behaviour, null);
                if(instance != null && returnType != typeof(void) && !_dependencies.ContainsKey(returnType))
                {
                    _dependencies.Add(returnType, instance);
                    Debug.Log($"[DI] {behaviour.name} provides {returnType.Name}");
                }
                else
                {
                    Debug.LogError($"[DI] {behaviour.name} cannot provide {returnType.Name}");
                }
            }
        }

        private void InjectsMethods(MonoBehaviour behaviour)
        {
            var methods = behaviour.GetType().GetMethods(k_bindingFlags)
                .Where(m => Attribute.IsDefined(m, typeof(InjectAttribute)));
            
            foreach (var method in methods)
            {
                Debug.Log(method.Name);
                var parameterTypes = method.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                if (parameterTypes.Length == 0) continue;

                var args = new object[parameterTypes.Length];
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    var type = parameterTypes[i];
                    if (_dependencies.TryGetValue(type, out var value))
                    {
                        args[i] = value;
                        Debug.Log($"[DI] {behaviour.name} injects {type.Name}");
                    }
                    else
                    {
                        Debug.LogError($"[DI] Missing dependency for {type.Name} in {behaviour.name}");
                    }
                }

                method.Invoke(behaviour, args);
            }
        }

        private void InjectsFields(MonoBehaviour behaviour)
        {
            var fields = behaviour.GetType().GetFields(k_bindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));

            foreach (var field in fields)
            {
                if(field.GetValue(behaviour) != null) continue;
                if (_dependencies.TryGetValue(field.FieldType, out var value))
                {
                    field.SetValue(behaviour, value);
                    Debug.Log($"[DI] {behaviour.name} injects {field.FieldType.Name}");
                }
                else
                {
                    Debug.LogError($"[DI] Missing dependency for {field.FieldType.Name} in {behaviour.name}");
                }
            }
        }

        private MonoBehaviour[] GetAllMonoBehaviours()
        {
            return GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public void ResetStaticField()
        {
            _dependencies.Clear();
        }
        
        public void LogDependencies()
        {
            if (_dependencies != null)
            {
                foreach (var dependency in _dependencies)
                {
                    Debug.Log("Logging dependencies:");
                    Debug.Log($"[DI] {dependency.Key.Name} : {dependency.Value}");
                }
            }
        }
    }
}