using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Bap.State_Machine.Player
{
    public class Grounded : BaseState
    {
        public Grounded(PlayerContext ctx, StateFactory _factory, bool isRoot) : base(ctx, _factory, isRoot) {}

        public override void Enter()
        {
            _ctx.Anim.SetBool(PlayerAnimationString.OnGrounded, true);
            InitializeSubState();
        }

        public override void Update(){}

        public override void Exit()
        {
            
        }

        protected override void CheckTransition()
        {
            if (!_ctx.OnGround)
            {
                _ctx.TransitionRootState(this, _factory.GetState<OnAir>());
            }
            else if (_ctx.IsRolling)
            {
                _ctx.TransitionRootState(this, _factory.GetState<Roll>());
            }
        }

        public override void InitializeSubState()
        {
            var state = _ctx.IsMoving
                ? _factory.GetState<Walk>()
                : _factory.GetState<Idle>();
                
                SetSubState(state);
                state.Enter();
                _ctx.SetCurrentSubState(state);
        }

    }
}