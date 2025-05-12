using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Utilities;

namespace Bap.Pool
{
    public interface IPoolObject<T>
    {
        public T OnCreateInstance();
        public void GetInstance(T instance);
        public void ReleaseInstance(T instance);
    }
    
    public abstract class Pool<T> : Singleton<Pool<T>>, IPoolObject<T> where T : class
    {
        [SerializeField] private int _capacity;
        [SerializeField] protected List<T> _list;
        
        protected ObjectPool<T> _myPool;
        public ObjectPool<T> MyPool => _myPool;

        public override void Awake()
        {
            base.Awake();
            _list = new();
            _myPool = new ObjectPool<T>(OnCreateInstance, GetInstance, ReleaseInstance, OnDestroyInsance, true, _capacity);
        }

        public abstract T OnCreateInstance();

        public abstract void GetInstance(T instance);

        public abstract void ReleaseInstance(T instance);
        public abstract void OnDestroyInsance(T instance);
    }
}