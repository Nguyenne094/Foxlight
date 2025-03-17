using System;
using UnityEngine;
using UnityEngine.Pool;
using Utilities;

namespace Bap.Pool
{
    public abstract class Pool<T> : Singleton<Pool<T>>, IPoolObject<T> where T : Component
    {
        protected ObjectPool<T> _myPool;
        public ObjectPool<T> MyPool => _myPool;

        private void Awake()
        {
            _myPool = new ObjectPool<T>(OnCreateInstance, GetInstance, ReleaseInstance);
        }

        public abstract T OnCreateInstance();

        public abstract void GetInstance(T instance);

        public abstract void ReleaseInstance(T instance);
    }
}