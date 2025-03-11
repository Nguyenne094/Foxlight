using System;
using System.Collections;
using System.Collections.Generic;
using Bap.System.Health;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace PlatformingGame.Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("References")]
        [SerializeField] private ControllerStatSO _colStat;
        [SerializeField] private JoyStick _joyStick;
        
        [Header("Movement")]
        [SerializeField] private bool _isMoving = false;
        [SerializeField] private int _moveInput;
        
        [Header("Rolling")]
        [SerializeField] private bool _isDashing = false;
        [SerializeField] private bool _canDash = true;

        [Header("Jumping")] 
        [SerializeField] private bool _onGround;
        
        [SerializeField, Tooltip("True while Player pressed jump and on ground before character is in air")] 
        private bool _isJumping;
        [SerializeField, Tooltip("If velocity y lower than 0")]
        private bool _isFalling;
        [SerializeField] private int _yVelocityDirection;
        [SerializeField, Tooltip("Period of time character is jumping, not fall. It resets to zero while peaking (y velocity = 0)")] 
        
        private float _computedGravity;
        private float _jumpVelocity;
        
        
        private Rigidbody2D _rb;
        private Collider2D _col;
        private Animator _animator;
        private Player _player;
        private FacingDirection _facingDirection = FacingDirection.Right;

        #region Properties

        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                _isMoving = value;
                _animator.SetBool(PlayerAnimationString.IsWalking, _isMoving);
            }
        }

        public bool IsJumping
        {
            get
            {
                return _isJumping;
            }
            set
            {
                _isJumping = value;
            }
        }

        public bool OnGround
        {
            get
            {
                return _onGround;
            }
            private set
            {
                _onGround = value;
                if (_onGround)
                {
                    IsJumping = false;
                }
                _animator.SetBool(PlayerAnimationString.OnGrounded, _onGround);
            }
        }
        
        #endregion
        
        private enum FacingDirection
        {
            Right = 1,
            Left = -1
        }
        
        public override void Awake()
        {
            base.Awake();
            _player = GetComponent<Player>();
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            GroundCheck();
            Move();
            ModifyJump();
        }

        private void FixedUpdate()
        {
            if (!_isDashing)
            {
                _rb.linearVelocity = new Vector2(_moveInput * _colStat.MoveSpeed, _rb.linearVelocity.y);
            }
        }
        
        public void Move()
        {
            _moveInput = _joyStick.GetNormalizedHorizontalMovement();
            
            IsMoving = _moveInput != 0;

            SetFacingDirection(_moveInput);
        }

        private void ModifyJump()
        {
            // ConfigGravity();
            ComputePhysicsParameters();
            RestrictFallinSpeed();
        }

        private void RestrictFallinSpeed()
        {
            if (_rb.linearVelocity.y < -_colStat.MaxFallSpeed)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_colStat.MaxFallSpeed);
            }
        }

        void ComputePhysicsParameters()
        {
            // Công thức tính: g = (2 * jumpHeight) / (jumpDuration^2)
            _computedGravity = (2 * _colStat.JumpHeight) / (_colStat.JumpDuration * _colStat.JumpDuration);
            // Vận tốc ban đầu: v0 = g * jumpDuration = 2 * jumpHeight / jumpDuration
            _jumpVelocity = _computedGravity * _colStat.JumpDuration;

            // Trong Unity, Physics2D.gravity mặc định là (0, -9.81).
            // Để nhân vật có gia tốc trọng lực hiệu dụng gia tốc computedGravity (hướng âm),
            // ta cập nhật gravityScale:
            _rb.gravityScale = _computedGravity / Mathf.Abs(Physics2D.gravity.y);
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.started && OnGround)
            {
                IsJumping = true;

                _rb.linearVelocityY = _jumpVelocity;
            }
        }

        public void Dash(InputAction.CallbackContext ctx)
        {
            if (ctx.started && !_isDashing && _canDash)
            {
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            StartCoroutine(DashCooldown(_colStat.RollDelay));
            _animator.SetTrigger(PlayerAnimationString.TriggerDash);
            _isDashing = true;
            _player.IsInvincible = true;
            
            //Ignore Layer
            Physics2D.IgnoreLayerCollision(gameObject.layer, 0, true);
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, true);
            
            float dashSpeed = (int)_facingDirection * _colStat.RollForce;
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocityY);
            _rb.linearVelocity = new Vector2(dashSpeed, _rb.linearVelocity.y);

            yield return new WaitForSeconds(_colStat.RollDuration);
            
            //Resest Ignore Layer
            Physics2D.IgnoreLayerCollision(gameObject.layer, 0, false);
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, false);
            
            _player.IsInvincible = false;
            _isDashing = false;
        }

        private IEnumerator DashCooldown(float delayTime)
        {
            _canDash = false;
            yield return new WaitForSeconds(delayTime);
            _canDash = true;
        }

        private void GroundCheck()
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            OnGround = _col.Cast(Vector2.down, _colStat.GroundFilter, hits, _colStat.GroundCheckDistance) > 0;
        }

        private void SetFacingDirection(int moveInput)
        {
            if (_animator.GetBool(PlayerAnimationString.CanChangeDirection) == true)
            {
                if (moveInput > 0 && _facingDirection == FacingDirection.Left)
                {
                    _facingDirection = FacingDirection.Right;
                }
                else if(moveInput < 0 && _facingDirection == FacingDirection.Right)
                {
                    _facingDirection = FacingDirection.Left;
                }
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (int)_facingDirection, transform.localScale.y);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_col)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_col.bounds.center + new Vector3(0, -_col.bounds.size.y/2), _col.bounds.center + new Vector3(0, -_col.bounds.size.y/2 - _colStat.GroundCheckDistance));
            }
        }
    }
}