using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Bap.Pool
{
    public abstract class Pool<T> : MonoBehaviour, IPoolObject<T> where T : MonoBehaviour
    {
        protected ObjectPool<T> MyPool;

        private void Awake()
        {
            MyPool = new ObjectPool<T>(OnCreate, Get, Release);
        }

        public abstract T OnCreate();

        public abstract void Get(T instance);

        public abstract void Release(T instance);
    }
}