using System;
using System.Collections.Generic;
using System.Linq;
using Bap.DependencyInjection;
using Bap.State_Machine.Player;
using Bap.System.Health;
using PlatformingGame.Controller;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Bap.State_Machine
{
    public enum FacingDirection
    {
        Right = 1,
        Left = -1
    }
    
    /// <summary>
    /// Manage context logic and the transitions of all states
    /// </summary>
    [RequireComponent(typeof(StateFactory))]
    public class PlayerContext : Singleton<PlayerContext>
    {
        [Header("Debug")]
        public List<string> StateList;
        [FormerlySerializedAs("CurrentStateView")] public string CurrentSuperStateString;
        [FormerlySerializedAs("CurrentSubStateView")] public string CurrentSubStateString;
        
        [FormerlySerializedAs("_stateFactory")]
        [Header("References")]
        [SerializeField] private StateFactory _factory;
        [SerializeField] private ControllerStatSO _controllerStatSo;
        [SerializeField] private Collider2D _col;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _anim;
        [SerializeField] private PlayerHealth _playerHealth;
        
        [Header("Movement")] 
        [SerializeField] private bool _canControl;
        [SerializeField] private bool _canMove;
        [SerializeField] private bool _isMoving = false;
        [SerializeField] private int _moveInput;
        
        [Header("Jumping")] 
        [SerializeField] private bool _onGround;
        
        [SerializeField, Tooltip("True while Player pressed jump and on ground before character is in air")] 
        private bool _isJumping;
        [SerializeField, Tooltip("If velocity y lower than 0")]
        private bool _isFalling;
        private float _jumpVelocity;
        
        [Header("Rolling")] [SerializeField]
        private bool _isRolling;
        
        [Header("Effects")] 
        [SerializeField] private ParticleSystem _footstepParticleSystem;
        

        [Inject] private JoyStick _joyStick;
        private FacingDirection _facingDirection = FacingDirection.Right;
        private float _computedGravity;
        private BaseState _currentSuperState;
        private BaseState _currentSubState;
        

        #region Properties

        public StateFactory StateFactory { get => _factory; }
        public PlayerHealth PlayerHealth { get => _playerHealth; }
        public ControllerStatSO ControllerStatSo { get => _controllerStatSo; }
        public FacingDirection FacingDirection => _facingDirection;
        public int MoveInput { get => _moveInput; }
        public BaseState CurrentSuperState
        {
            get => _currentSuperState;
            private set
            {
                _currentSuperState = value;
                CurrentSuperStateString = _currentSuperState.GetType().Name;
            }
        }
        public BaseState CurrentSubState
        {
            get => _currentSubState;
            private set
            {
                _currentSubState = value;
                CurrentSubStateString = _currentSubState.GetType().Name;
            }
        }
        public Collider2D Col { get => _col; }
        public Rigidbody2D Rb { get => _rb; }
        public Animator Anim { get => _anim; }
        public bool OnGround
        {
            get
            {
                return _onGround;
            }
            private set
            {
                _onGround = value;
                if (_onGround && _rb.linearVelocityY <= 0f)
                {
                    IsJumping = false;
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
                        _footstepParticleSystem?.Play();
                    }
                    else
                    {
                        _footstepParticleSystem?.Stop();
                    }
                }
            }
        }
        public bool IsFalling { get => _isFalling; set => _isFalling = value; }
        public bool IsRolling { get => _isRolling; set => _isRolling = value; }
        public bool CanMove
        {
            get => _canMove;
            set
            {
                _canMove = value;
                Anim.SetBool(PlayerAnimationString.CanMove, _canMove);
            }
        }
        public bool CanControl { get => _canControl; set => _canControl = value; }
        
        #endregion

        public override void Awake()
        {
            base.Awake();
            ComputePhysicsParameters();
            _footstepParticleSystem ??= GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            StateList = _factory.GetStateList();
            SetCurrentSuperState(_factory.GetState<Grounded>());
            CurrentSuperState.Init();
        }

        private void Update()
        {
            GroundCheck();
            Move();
            CurrentSuperState?.UpdateStates();
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                if (!_controllerStatSo.Roll?.Using ?? true)
                {
                    _rb.linearVelocity = new Vector2(_moveInput * _controllerStatSo.MoveSpeed,_rb.linearVelocityY);
                }
            }
        }

        public void TransitionRootState(BaseState from, BaseState to)
        {
            if(to == null || from == null)
            {
                Debug.LogError($"State {from.GetType().Name} or {to.GetType().Name} is null");
                return;
            }

            if (!from.IsRootState || !to.IsRootState)
            {
                Debug.LogError($"Transition state fail. State {from.GetType().Name} and {to.GetType().Name} is not root state ");
                return;
            }
            
            from.Exit();
            from.CurrentSubState?.Exit();
            SetCurrentSuperState(to);
            to.Enter();
        }
        
        public void TransitionSubState(BaseState from, BaseState to)
        {
            if(to == null || from == null)
            {
                Debug.LogError($"State {from.GetType().Name} or {to.GetType().Name} is null");
                return;
            }

            if (from.IsRootState || to.IsRootState)
            {
                Debug.LogError($"Transition state fail. State {from.GetType().Name} and {to.GetType().Name} is not subState");
            }

            from.Exit();
            SetCurrentSubState(to);
            to.Enter();
        }
        
        public void SetCurrentSuperState(BaseState state)
        {
            if (state != null)
            {
                CurrentSuperState = state;
            }
            else
            {
                //TODO: Switch to empty state to avoid crashing game
                Debug.LogError($"[HFSM] {CurrentSuperState.GetType().Name} switchs to null state");
            }
        }
        public void SetCurrentSubState(BaseState state)
        {
            if (state != null)
            {
                CurrentSubState = state;
            }
            else
            {
                //TODO: Switch to empty state to avoid crashing game
                Debug.LogError($"[HFSM] {CurrentSubState.GetType().Name} switchs to null state");
            }
        }
        
        public void Move()
        {
            _moveInput = _joyStick?.GetNormalizedHorizontalMovement() ?? 0;
            
            IsMoving = (_moveInput != 0) && CanControl;

            SetFacingDirection(_moveInput);
        }
        
        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.started && OnGround)
            {
                _rb.linearVelocityY = _jumpVelocity;
                _isJumping = true;
            }
        }
        
        public void Roll(InputAction.CallbackContext ctx)
        {
            if(ctx.started && _controllerStatSo.Roll != null && _controllerStatSo.Roll.CanUse)
            {
                TransitionSubState(CurrentSubState, _factory.GetState<Roll>());
            }
        }
        
        private void GroundCheck()
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            OnGround = Col.Cast(Vector2.down, ControllerStatSo.GroundFilter, hits, ControllerStatSo.GroundCheckDistance) > 0;
        }
        
        void ComputePhysicsParameters()
        {
            // Công thức tính: g = (2 * jumpHeight) / (jumpDuration^2)
            _computedGravity = (2 * _controllerStatSo.JumpHeight) / (_controllerStatSo.JumpDuration * _controllerStatSo.JumpDuration);
            // Vận tốc ban đầu: v0 = g * jumpDuration = 2 * jumpHeight / jumpDuration
            _jumpVelocity = _computedGravity * _controllerStatSo.JumpDuration;
            
            // Để nhân vật có gia tốc trọng lực hiệu dụng gia tốc computedGravity (hướng âm),
            // ta cập nhật gravityScale:
            _rb.gravityScale = _computedGravity / Mathf.Abs(Physics2D.gravity.y);
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
    }
}