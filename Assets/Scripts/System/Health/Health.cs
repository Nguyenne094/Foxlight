using System;
using UnityEngine;

namespace Bap.System.Health
{
    public interface IHealth
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public bool IsAlive { get; set; }

        public void TakeDamage(int damage, Transform subject);
    }
    
    public abstract class Health: MonoBehaviour, IHealth 
    {
        [Header("Heath Configs")]
        [SerializeField] protected int _currentHealth;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected bool _isAlive = true;
        public int CurrentHealth 
        {
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
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }

        protected virtual void Awake()
        {
            ResetHealth();
        }

        public abstract void TakeDamage(int damage, Transform subject);
        
        public void ResetHealth() => CurrentHealth = MaxHealth;
    }
}