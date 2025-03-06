using System;
using Bap.System.Health;
using UnityEngine;
using Others;

namespace Others
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private Lever _lever;
        [SerializeField] private int _damage;
        [SerializeField, Min(0), Tooltip("Target gets damage if fall velocity (Y) lower than threshold. Threshold is seted negative in code")] 
        private float _threshold;
        

        private Rigidbody2D _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
            _lever.OnActive += DropLog;
            _rb.bodyType = RigidbodyType2D.Static;
        }

        private void DropLog()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_rb.linearVelocityY > -_threshold &&
                other.gameObject.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(_damage);
                FallIntoTarget();
            }
        }

        private void FallIntoTarget()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Break");
            GetComponent<Collider2D>().isTrigger = true;
            Destroy(this.gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
        }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (_rb.linearVelocityY < _threshold &&
        //         other.TryGetComponent<Health>(out Health health))
        //     {
        //         health.TakeDamage(_damage);
        //     }
        // }
    }
}