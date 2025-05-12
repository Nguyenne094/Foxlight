using System;
using System.Collections.Generic;
using System.Collections;
using Bap.System.Health;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace PlatformingGame.Controller
{
    [CreateAssetMenu(fileName = "Roll", menuName = "Player Ability/Roll", order = 0)]
    public class RollAbilitySO : PlayerAbilityScriptableObject
    {
        [Header("Rolling Configs")]
        [Min(0.1f)] public float RollForce = 5f;
        [Min(0.1f)] public float RollTime = 0.1f;

        private Rigidbody2D Rb => _controller.Rb;
        private Animator Anim => _controller.Anim;
        private PlayerHealth PlayerHealth => _controller.PlayerHealth;


        public override void OnStart()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void Active()
        {
            if (!Using && CanUse)
            {
                Roll();
            }
        }

        public override void Restart() { }

         private void Roll()
         {
             StartCoolDown();
             Anim.SetTrigger(PlayerAnimationString.TriggerDash);
             Using = true;
             PlayerHealth.IsInvincible = true;
        
             //Ignore Layer
             Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Default"), true);
             Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Enemy Hurt Box"), true);
        
             float dashSpeed = (int)_controller.GetFacingDirection * RollForce;
             Rb.linearVelocity = new Vector2(0, _controller.Rb.linearVelocityY);
             Rb.linearVelocity = new Vector2(dashSpeed, _controller.Rb.linearVelocity.y);
        
             DOVirtual.DelayedCall(RollTime, () =>
             {
                 //Resest Ignore Layer
                 Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Default"), false);
                 Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Enemy Hurt Box"), false);
        
                 PlayerHealth.IsInvincible = false;
                 Using = false;
             }, false);
         }
    }
}