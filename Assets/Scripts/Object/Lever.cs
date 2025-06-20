﻿using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Lever just can be actived by falling into it
/// </summary>
public class Lever : MonoBehaviour
{
    [SerializeField] private LayerMask _activedByLayer;
    [SerializeField] private Log _activeLog;

    public Action OnActive;
    
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogError(gameObject.name + ": Missing ref to Animator");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & _activedByLayer.value) != 0 &&
            other.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            if (rb.linearVelocityY < 0) // means other is falling
            {
                _animator.SetTrigger("Active");
                OnActive?.Invoke();
                DOVirtual.DelayedCall(_activeLog.RecreateAfter, () => _animator.SetTrigger("Return"));
            }
        }
    }
}
