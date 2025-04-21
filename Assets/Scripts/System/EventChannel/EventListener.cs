using UnityEngine.Events;
using UnityEngine;

namespace System.EventChannel
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        public EventChannelSO<T> EventChannel;
        public UnityEvent<T> Response;
        
        private void OnAwake()
        {
            if (EventChannel != null)
            {
                EventChannel.Register(this);
            }
        }
        
        private void OnDestroy()
        {
            if (EventChannel != null)
            {
                EventChannel.Unregister(this);
            }
        }
        
        public void Invoke(T value)
        {
            Response?.Invoke(value);
        }
    }
}