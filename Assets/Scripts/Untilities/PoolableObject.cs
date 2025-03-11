using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Utilities
{
    /// <summary>
    /// GameObject pools itself when disabled.
    /// You must initilize ObjectPool Parent and its event functions 
    /// </summary>
    public class PoolableObject : MonoBehaviour
    {
        public static ObjectPool<GameObject> Parent;
        public event Action OnRelease;

        private bool isReleased;
        public bool IsReleased { get => isReleased; set => isReleased = value; }

        private void OnDisable()
        {
            if (!IsReleased)
            {
                Parent.Release(this.gameObject);
                OnRelease?.Invoke();
                IsReleased = true;
            }
        }

        private void OnDestroy()
        {
            if (!IsReleased)
            {
                Parent.Release(this.gameObject);
                OnRelease?.Invoke();
                IsReleased = true;
            }
        }
    }
}