using System;
using Scriptable_Object.Event;
using Unity.Behavior;
using UnityEngine;

namespace Bap.System.Health
{
    public class Player : Health
    {
        [Header("Events")]
        [SerializeField] private IntEventChannelSO OnTakeDamage;
        [SerializeField] private VoidEventChannelSO OnDeath;

        private bool _isInvincible = false;

        public bool IsInvincible { get; set; }
        
        public override void TakeDamage(int value)
        {
            if (!_isInvincible)
            {
                CurrentHealth -= value;
                if (CurrentHealth == 0)
                {
                    _isAlive = false;
                    OnDeath?.RaiseEvent();
                    //TODO: Play sfx, vfx
                }
                else
                {
                    OnTakeDamage.RaiseEvent(value);
                    //TODO: Play sfx, vfx
                }
            }
        }
    }
}