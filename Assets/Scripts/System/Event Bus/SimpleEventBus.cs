using UnityEngine;

namespace System.EventBus
{
    public class SimpleEventBus : MonoBehaviour
    {
        public delegate void EventHandler<T>(T eventData);
    }
}