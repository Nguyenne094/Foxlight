using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Untilities
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;
        [SerializeField] private LayerMask _interactByLayer;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _interactByLayer) != 0)
            {
                _event.Invoke();
            }
        }
    }
}