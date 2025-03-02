using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bap.System.Health
{
    public class Enemy : Health
    {
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private bool _isInvincible;
        
        [Header("Events")]
        [SerializeField] private DamgeTakenEvent OnTakeDamage;
        [SerializeField] private DieEvent OnDeath;

        protected override void Awake()
        {
            base.Awake();
            _targetLayer = LayerMask.GetMask("Player");
        }

        public override void TakeDamage(int value)
        {
            if (!_isInvincible)
            {
                CurrentHealth -= value;
                if (CurrentHealth == 0)
                {
                    _isAlive = false;
                    OnDeath.SendEventMessage(this.gameObject);
                }
                else
                {
                    OnTakeDamage.SendEventMessage(this.gameObject, value);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player)
                {
                    player.TakeDamage(Damage);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player)
                {
                    player.TakeDamage(Damage);
                }
            }
        }
    }
}