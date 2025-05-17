using UnityEngine;

namespace Bap.State_Machine.Player
{
    public class Jump : BaseState
    {
        public Jump(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot)
        {
        }

        public override void Enter()
        {
            //Set Animator
            _ctx.IsJumping = true;
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            _ctx.IsJumping = false;
        }

        protected override void CheckTransition()
        {
            if(_ctx.Rb.linearVelocityY <= 0)
                _ctx.TransitionSubState(this, _factory.GetState<Fall>());
        }
    }
}