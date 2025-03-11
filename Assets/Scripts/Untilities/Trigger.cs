using System;
using UnityEngine;
using UnityEngine.Events;

namespace Untilities
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent _event;//TODO: Xài tạm vì không áp được hiệu ứng fade
        [SerializeField] private LayerMask _interactorLayer;
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _interactorLayer) != 0)
            {
                _event.Invoke();
            }
        }
    }
}