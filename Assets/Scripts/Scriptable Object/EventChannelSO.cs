using UnityEngine;
using UnityEngine.Events;

namespace Scriptable_Object.Event
{
    /// <summary>
    /// Other type of T just need to change to resonable T type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        [Tooltip("The action to perform")]
        public UnityAction<T> OnEventRaised;

        public GameObject GetRaiser()
        {
            return null;
        }

        public void RaiseEvent(T value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}