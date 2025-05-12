using System;
using Bap.Pool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bap.System.Health
{
    public class EnemyHealth : Health
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private bool _isInvincible;
        
        [Header("Events")]
        [SerializeField] private DamageTakenEvent OnTakeDamage;
        [SerializeField] private DieEvent OnDeath;

        protected override void Awake()
        {
            base.Awake();
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
    }
}