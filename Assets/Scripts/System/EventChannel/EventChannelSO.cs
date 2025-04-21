using System.Collections.Generic;
using UnityEngine;

namespace System.EventChannel
{
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        readonly HashSet<EventListener<T>> Listeners = new();

        public void Raise(T value)
        {
            foreach (var listener in Listeners)
            {
                listener.Invoke(value);
            }
        }

        public void Register(EventListener<T> listener)
        {
            if(!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }
        
        public void Unregister(EventListener<T> listener)
        {
            if(Listeners.Contains(listener))
            {
                Listeners.Remove(listener);
            }
        }
    }
    
    public struct Empty{}
    
    [CreateAssetMenu(fileName = "NewIntEventChannel", menuName = "EventChannel/IntEventChannel")]
    public class IntEventChannelSO : EventChannelSO<int> { }
    
    [CreateAssetMenu(fileName = "NewFloatEventChannel", menuName = "EventChannel/FloatEventChannel")]
    public class FloatEventChannelSO : EventChannelSO<float> { }
    
    [CreateAssetMenu(fileName = "NewVoidEventChannel", menuName = "EventChannel/VoidEventChannel")]
    public class VoidEventChannelSO : EventChannelSO<Empty> { }
}