using System;
using System.Collections;
using System.Collections.Generic;
using Bap.Service_Locator;
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
        [SerializeField] private ControllerStatSO _conStat;
        
        [Header("Movement")] 
        [SerializeField] private bool _canControl;
        [SerializeField] private bool _isMoving = false;
        [SerializeField] private int _moveInput;

        [Header("Jumping")] 
        [SerializeField] private bool _onGround;
        
        [SerializeField, Tooltip("True while Player pressed jump and on ground before character is in air")] 
        private bool _isJumping;
        [SerializeField, Tooltip("If velocity y lower than 0")]
        private bool _isFalling;
        [SerializeField] private int _yVelocityDirection;

        [Header("Effects")] 
        [SerializeField] private ParticleSystem _footstepParticleSystem;
        
        private float _computedGravity;
        private float _jumpVelocity;
        private bool _canMove = true;
        
        private JoyStick _joyStick;
        private Rigidbody2D _rb;
        private Collider2D _col;
        private Animator _anim;
        private Player _player;
        private FacingDirection _facingDirection = FacingDirection.Right;
        
        #region Properties

        public bool CanMove
        {
            get => _canMove;
            set
            {
                _canMove = value;
                Anim.SetBool("CanMove", _canMove);
            }
        }

        public bool CanControl { get => _canControl; set => _canControl = value; }
        public int MoveInput => _moveInput;
        public ControllerStatSO ConStat => _conStat;
        public Player Player => _player;
        public FacingDirection GetFacingDirection => _facingDirection;
        public Rigidbody2D Rb { get => _rb; set => _rb = value; }
        public Animator Anim { get => _anim; set => _anim = value; }
        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                bool moving = value && _rb.linearVelocityX != 0;
                if (_isMoving != moving)
                {
                    _isMoving = moving;
                    _anim.SetBool(PlayerAnimationString.IsWalking, moving);
                    if (moving)
                    {
                        _footstepParticleSystem.Play();
                    }
                    else
                    {
                        _footstepParticleSystem.Stop();
                    }
                }
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
                _anim.SetBool(PlayerAnimationString.OnGrounded, _onGround);
            }
        }
        
        #endregion
        
        public enum FacingDirection
        {
            Right = 1,
            Left = -1
        }
        
        public override void Awake()
        {
            base.Awake();
            if(_footstepParticleSystem == null) _footstepParticleSystem = GetComponentInChildren<ParticleSystem>();
            _player = GetComponent<Player>();
            _anim = GetComponent<Animator>();
            _col = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            
            _conStat.Init(this);
        }

        private void Start()
        {
            ServiceLocator.Global.Get(out _joyStick);
        }

        private void Update()
        {
            GroundCheck();
            Move();
            ComputePhysicsParameters();
            RestrictFallingSpeed();
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                if (_conStat.Roll != null)
                {
                    if(!_conStat.Roll.Using)
                    {
                        _rb.linearVelocity = new Vector2(MoveInput * _conStat.MoveSpeed, _rb.linearVelocityY);
                    }

                    return;
                }
                _rb.linearVelocity = new Vector2(MoveInput * _conStat.MoveSpeed, _rb.linearVelocityY);  
            }
        }
        
        public void Move()
        {
            _moveInput = _joyStick.GetNormalizedHorizontalMovement();
            
            IsMoving = (_moveInput != 0) && CanControl;

            SetFacingDirection(_moveInput);
        }

        private void RestrictFallingSpeed()
        {
            if (_rb.linearVelocity.y < -_conStat.MaxFallSpeed)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_conStat.MaxFallSpeed);
            }
        }

        void ComputePhysicsParameters()
        {
            // Công thức tính: g = (2 * jumpHeight) / (jumpDuration^2)
            _computedGravity = (2 * _conStat.JumpHeight) / (_conStat.JumpDuration * _conStat.JumpDuration);
            // Vận tốc ban đầu: v0 = g * jumpDuration = 2 * jumpHeight / jumpDuration
            _jumpVelocity = _computedGravity * _conStat.JumpDuration;

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

        public void Roll(InputAction.CallbackContext ctx)
        {
            if(ctx.started && _conStat.Roll != null) _conStat.Roll.Active();
        }

        private void GroundCheck()
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            OnGround = _col.Cast(Vector2.down, _conStat.GroundFilter, hits, _conStat.GroundCheckDistance) > 0;
        }

        private void SetFacingDirection(int moveInput)
        {
            if (_anim.GetBool(PlayerAnimationString.CanChangeDirection) == true)
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
                Gizmos.DrawLine(_col.bounds.center + new Vector3(0, -_col.bounds.size.y/2), _col.bounds.center + new Vector3(0, -_col.bounds.size.y/2 - _conStat.GroundCheckDistance));
            }
        }
    }
}