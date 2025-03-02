using UnityEngine;
using UnityEngine.Events;

namespace Scriptable_Object.Event
{
    [CreateAssetMenu(fileName = "New Void Event Channel", menuName = "Event Channel/Void", order = 0)]
    public  class VoidEventChannelSO : ScriptableObject
    {
        [Tooltip("The action to perform")]
        public UnityAction OnEventRaised;

        public GameObject GetRaiser()
        {
            return null;
        }

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
        }
    }
}