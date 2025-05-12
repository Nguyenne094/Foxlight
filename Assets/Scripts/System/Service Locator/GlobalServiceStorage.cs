using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bap.Service_Locator
{
    public class GlobalServiceStorage : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private List<Object> _services;
        
        private void Awake()
        {
            Debug.Log("ServiceStorage: Start registering services for global");
            foreach (var service in _services)
            {
                ServiceLocator.Global.Register(service.GetType(), service);
            }
        }

#if UNITY_EDITOR
        // [MenuItem("GameObject/ServiceLocator/Add Global ServiceStorage")]
        // public static void CreateServiceStorage()
        // {
        //     var container = new GameObject("ServiceStorage [Global]", typeof(GlobalServiceStorage));
        // }
#endif
    }
}