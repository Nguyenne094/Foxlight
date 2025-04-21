using UnityEngine;
using UnityEngine.Pool;

namespace Bap.Pool
{
    public interface IPoolObject<T>
    {
        public T OnCreateInstance();
        public void GetInstance(T instance);
        public void ReleaseInstance(T instance);
    }
}