using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace PlatformingGame.Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        //TODO: Anti Gravity Apex
        //TODO: Early Fall
        //TODO: Jump Buffering
        //TODO: Sticky Feet On Land
        //TODO: Speed Apex
        //TODO: Coyote time
        //TODO: Clamp Falling Speed
        //TODO: Catch Miss Jump
        //TODO: Bumped Head Correction
        //TODO: Corner Clip On Jump
        //TODO: Hold Crunch On Stay On Ledge
        //TODO: Relaxed Semi-Solid
        //TODO: Variable Jump Height

        [SerializeField] private ControllerStatSO _colStat;
        [SerializeField] private JoyStick _joyStick;
        
        
        private Rigidbody2D _rb;
        private Collider2D _col;
        private Animator _animator;
        private FacingDirection _facingDirection = FacingDirection.Right;
        
        private bool _isMoving = false;
        private bool _isDashing = false;
        private bool _canDash = true;
        
        private int _moveInput;
        
        private enum FacingDirection
        {
            Right = 1,
            Left = -1
        }
        
        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            private set
            {
                _isMoving = value;
                _animator.SetBool(PlayerAnimationString.IsWalking, _isMoving);
            }
        }
        
        public override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(PlayerAnimationString.OnGrounded, IsGround());
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

        private void ModifyJump()
        {
            //Fall and normal gravity
            if (_rb.linearVelocityY < 0)
            {
                _rb.gravityScale = _colStat.FallMultiplier;
            }
            else
            {
                _rb.gravityScale = _colStat.NormalMultiplier;
            }
            
            //Clamp falling speed
            if (_rb.linearVelocity.y < -_colStat.MaxFallSpeed)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_colStat.MaxFallSpeed);
            }
        }

        public void Move()
        {
            _moveInput = _joyStick.GetNormalizedHorizontalMovement();
            
            IsMoving = _moveInput != 0;

            SetFacingDirection(_moveInput);
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.started && IsGround())
            {
                float jumpVel = Mathf.Sqrt(2 * -Physics2D.gravity.y * _colStat.JumpHeight);

                _rb.linearVelocityY = jumpVel;
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
            StartCoroutine(DashCooldown(_colStat.DashDelay));
            _isDashing = true;
            _animator.SetTrigger(PlayerAnimationString.TriggerDash);
            float dashSpeed = (int)_facingDirection * _colStat.DashForce;
            _rb.linearVelocity = new Vector2(dashSpeed, _rb.linearVelocity.y);

            yield return new WaitForSeconds(_colStat.DashDuration);
            
            _isDashing = false;
        }

        private IEnumerator DashCooldown(float delayTime)
        {
            _canDash = false;
            yield return new WaitForSeconds(delayTime);
            _canDash = true;
        }

        private bool IsGround()
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            return _col.Cast(Vector2.down, _colStat.ContactFilter2D, hits, 0.02f) > 0;
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
    }
}