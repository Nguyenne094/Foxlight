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
        [Min(0.1f), SerializeField] private float RollForce = 5f;

        private Rigidbody2D Rb => _controller.Rb;
        private Animator Anim => _controller.Anim;
        private PlayerHealth PlayerHealth => _controller.PlayerHealth;


        protected override void Action()
        {
            StartCoolDown();
            Anim.SetTrigger(PlayerAnimationString.TriggerDash);
            PlayerHealth.IsInvincible = true;

            // Ignore Layer
            Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Default"), true);
            Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Enemy Hurt Box"), true);

            float dashSpeed = Mathf.Sign((int)_controller.FacingDirection) * RollForce;
            Rb.linearVelocity = new Vector2(0, _controller.Rb.linearVelocityY);
            
            DOVirtual.Float(0, 1, CoolDownDuration, value =>
            {
                Debug.Log("Rolling");
                // Áp dụng vận tốc Dash trong suốt thời gian RollDuration
                Rb.linearVelocity = new Vector2(dashSpeed, _controller.Rb.linearVelocity.y);
            }).OnComplete(() =>
            {
                Debug.Log("Reset");
                Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Default"), false);
                Physics2D.IgnoreLayerCollision(_controller.gameObject.layer, LayerMask.NameToLayer("Enemy Hurt Box"), false);
                PlayerHealth.IsInvincible = false;
                Rb.linearVelocity = new Vector2(0, _controller.Rb.linearVelocityY);
            });
        }
    }
}