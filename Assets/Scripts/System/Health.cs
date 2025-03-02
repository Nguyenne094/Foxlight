using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Bap.System.Health
{
    public abstract class Health: MonoBehaviour 
    {
        [Header("Heath Configs")]
        [SerializeField] protected int _currentHealth;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected bool _isAlive = true;
        [SerializeField] private int _damage;


        public int CurrentHealth {
            get
            {
                return _currentHealth;
            }
            set
            {
                _currentHealth = value;
                if (_currentHealth < 0) _currentHealth = 0;
                else if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
            }
        }
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
                if (_maxHealth <= 0) _maxHealth = 1;
                else if (_maxHealth > 1000) _maxHealth = 999;
            }
        }
        public int Damage => _damage;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public abstract void TakeDamage(int damage);
    }
}