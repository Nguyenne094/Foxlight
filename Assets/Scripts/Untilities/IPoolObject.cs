using UnityEngine;
using UnityEngine.Pool;

namespace Bap.Pool
{
    public interface IPoolObject<T>
    {
        public T OnCreate();
        public void Get(T instance);
        public void Release(T instance);
    }
}