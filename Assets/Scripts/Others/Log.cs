using System;
using System.Collections;
using System.Reflection;
using Bap.Pool;
using Bap.System.Health;
using DG.Tweening;
using UnityEngine;
using Untilities;

namespace Others
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private LogPool _pool;
        [SerializeField] private Lever _lever;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private LayerMask _interactiveLayer;
        [SerializeField, Min(0), Tooltip("Target gets damage if fall velocity (Y) lower than threshold. Threshold is seted negative in code")]
        private float _threshold;
        [SerializeField] private int _damage;
        [SerializeField] private float _recreateAfter;

        private bool _isBroken = false;
        
        private Vector2 _initialPosition;
        private Vector3 _initialRotation;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        public float RecreateAfter => _recreateAfter;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (!_pool)
            {
                _pool = FindFirstObjectByType<LogPool>();
                if(!_pool)
                    Debug.LogError("Log Pool don't exist");
            }
            if (!_particleSystem)
            {
                _particleSystem = GetComponentInChildren<ParticleSystem>();
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _lever.OnActive += DropLog;
            _initialPosition = transform.position;
            _initialRotation = transform.eulerAngles;
        }
        
        private void OnEnable()
        {
            RestartState();
        }

        private void DropLog()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        private void RestartState()
        {
            transform.position = _initialPosition;
            transform.eulerAngles = _initialRotation;
            _spriteRenderer.enabled = true;
            _isBroken = false;
            _rb.bodyType = RigidbodyType2D.Static;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((((1 << other.gameObject.layer) & _interactiveLayer.value) != 0)) && !_isBroken)
            {
                if(other.gameObject.TryGetComponent<Health>(out Health health))
                    health.TakeDamage(_damage, transform);
                ReadyToDisable();
                _isBroken = true;
                CameraShake.Instance.LightShake(0.2f);
            }
        }

        private void ReadyToDisable()
        {
            _spriteRenderer.enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;
            _particleSystem.Play();
            DOVirtual.DelayedCall(_particleSystem.main.startLifetime.constant, () => _pool.ReleaseInstance(this));
        }
    }
}