using System;
using Bap.Pool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bap.System.Health
{
    public class Enemy : Health
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private bool _isInvincible;
        
        [Header("Events")]
        [SerializeField] private DamgeTakenEvent OnTakeDamage;
        [SerializeField] private DieEvent OnDeath;
        
        [Header("Effects")] 
        
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _targetLayer = LayerMask.GetMask("Player");
        }

        public override void TakeDamage(int value, Transform transform)
        {
            if (!_isInvincible)
            {
                CurrentHealth -= value;
                if (CurrentHealth == 0)
                {
                    IsAlive = false;
                    OnDeath.SendEventMessage(_parent);
                }
                else
                {
                    OnTakeDamage.SendEventMessage(_parent, value);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player)
                {
                    player.TakeDamage(Damage, transform);
                }
            }
        }
    }
}