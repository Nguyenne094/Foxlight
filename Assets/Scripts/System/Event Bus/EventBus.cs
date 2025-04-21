using UnityEngine;
using System.Collections.Generic;

namespace System.EventBus
{
    public interface IEvent { }

    /// <summary>
    /// A static class that represents an event bus for a specific event type.
    /// Provides functionality to register, deregister, and raise events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of event, which must implement the <see cref="IEvent"/> interface.</typeparam>
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
        
        public static void Register(EventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

        /// <summary>
        /// Raises an event of type <typeparamref name="T"/> and invokes all registered bindings.
        /// </summary>
        /// <param name="event">The event instance to raise.</param>
        public static void Raise(T @event)
        {
            var snapshot = new HashSet<IEventBinding<T>>(bindings);

            foreach (var binding in snapshot)
            {
                if (bindings.Contains(binding))
                {
                    binding.OnEvent.Invoke(@event);
                    binding.OnEventNoArgs.Invoke();
                }
            }
        }
        
        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }
    }
}