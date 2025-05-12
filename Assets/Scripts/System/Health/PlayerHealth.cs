using System;
using System.Collections.Generic;
using DG.Tweening;
using PlatformingGame.Controller;
using Bap.EventChannel;
using Bap.Manager;
using Bap.Pool;
using UnityEngine.Serialization;
using UnityEngine.Pool;
using UnityEngine;
using Unity.Behavior;
using Utilities;
using Object = UnityEngine.Object;

namespace Bap.System.Health
{
    public class PlayerHealth : Health
    {
        
        [Header("Events")]
        [SerializeField] private IntEventChannelSO OnTakeDamage;
        [SerializeField] private VoidEventChannelSO OnDeath;
        
        [Header("Effects")]
        [SerializeField, Min(0)] private float _reviveAfter;
        [Min(0), Tooltip("Push force for TakeDamage effect")] 
        [SerializeField] private float _pushForce;
        [SerializeField] private ParticleSystem _deathParticleSystem;
        [SerializeField, Min(0)] private int _flashTimes;
        [SerializeField] private Color _flashColor;
        [SerializeField, Min(0)] private float _freezeTime;
        
        private ObjectPool<ParticleSystem> _deathPSPool;
        private bool _isInvincible = false;

        private Rigidbody2D _rb;
        private Animator _anim;
        private SpriteRenderer _sprite;
        
        public bool IsInvincible { get => _isInvincible; set => _isInvincible = value; }

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _deathPSPool =
                new ObjectPool<ParticleSystem>(CreateParticle, GetParticle, ReleaseParticle, DestroyParticle, true);
        }

        #region HP & Effect Logic

        public override void TakeDamage(int damage,Transform subject)
        {
            if (!IsInvincible && IsAlive)
            {
                var isHazard = subject.gameObject.layer == LayerMask.NameToLayer("Hazard");
                CurrentHealth -= damage;
                
                if (CurrentHealth <= 0)
                {
                    OnDeath?.RaiseEvent();
                    IsAlive = false;
                    DieEffects();
                }
                else
                {
                    OnTakeDamage.RaiseEvent(damage);
                    TakeDamageEffects(subject);
                    if (isHazard)
                    {
                        TakeDamageFromHazard(subject);
                    }
                }
            }
        }

        private void TakeDamageEffects(Transform subject)
        {
            IsInvincible = true;
            PlayerController.Instance.CanMove = false;

            var dir = Utils.GetDirectionVector2(subject.position, transform.position);
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = Vector2.one * dir * _pushForce;
            DOVirtual.DelayedCall(0.5f, () => PlayerController.Instance.CanMove = true, false);
            
            //!!! IMPORTANT !!! If Player stays in a enemy object and not moves, OnTriggerStay will not work
            
            //if invincible time is longer than sleep time (Project Settings -> Physics2D -> Time To Sleep)
            //it is related to Sleeping Mode in Rigidbody2D component: Sleeping Mode is a optimizing feature for object not moving
            DOVirtual.DelayedCall(Mathf.Min(1f, Physics2D.timeToSleep), () => IsInvincible = false, false);
            
            // // Freeze Time
            //  Time.timeScale = 0;
            //  DOVirtual.DelayedCall(_freezeTime, () => Time.timeScale = 1);
            
            //Camera Shake
            CameraShake.Instance.LightShake(0.2f);
            
            //Color Flash
            Sequence colorChangeSequence = DOTween.Sequence();
            colorChangeSequence
                .AppendCallback((() => _sprite.color = _flashColor))
                .AppendInterval(0.1f)
                .AppendCallback((() => _sprite.color = Color.white))
                .AppendInterval(0.1f)
                .SetLoops(_flashTimes, LoopType.Incremental);
        }

        private void DieEffects()
        {
            //TODO: Play sfx, vfx
            //Logic
            _rb.bodyType = RigidbodyType2D.Static;
            
            //Visual
            
            var particle = _deathPSPool.Get();
            Destroy(this.gameObject, particle.main.startLifetime.constant);
        }

        private void TakeDamageFromHazard(Transform subject)
        {
            DOVirtual.DelayedCall(_reviveAfter, () =>
            {
                transform.position = GameManager.Instance.LastCheckPoint;
            });
        }

        #endregion

        #region Pool Methods

        private void DestroyParticle(ParticleSystem obj)
        {
            Destroy(obj.gameObject);
        }

        private void ReleaseParticle(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);
            obj.Stop();
        }

        private void GetParticle(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);
            obj.Stop();
            obj.Play();
            obj.transform.position = transform.position; // + offet
            DOVirtual.DelayedCall(obj.main.startLifetime.constant, () => _deathPSPool.Release(obj));
        }

        private ParticleSystem CreateParticle()
        {
            return Object.Instantiate(_deathParticleSystem);
        }

        #endregion
    }
}