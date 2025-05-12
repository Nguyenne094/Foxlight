using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Bap.Pool
{
    /// <summary>
    /// Have 2 functions: Release is disable object, Get is enable object
    /// </summary>
    public static class QuickPool
    {
        private static Dictionary<GameObject, ObjectPool<GameObject>> _poolManager;

        static QuickPool()
        {
            _poolManager = new Dictionary<GameObject, ObjectPool<GameObject>>();
        }
        
        public static void Release(this GameObject obj)
        {
            if (!_poolManager.ContainsKey(obj))
            {
                _poolManager.Add(obj, new ObjectPool<GameObject>(
                    () => Object.Instantiate(obj), 
                    obj => obj.SetActive(true), 
                    obj => obj.SetActive(false)));
            }
            _poolManager[obj].Release(obj);
        }

        public static void Get(this GameObject obj)
        {
            if (!_poolManager.ContainsKey(obj))
            {
                _poolManager.Add(obj, new ObjectPool<GameObject>(
                    () => Object.Instantiate(obj), 
                    obj => obj.SetActive(true), 
                    obj => obj.SetActive(false)));
            }
            _poolManager[obj].Get();
        }
    }
}