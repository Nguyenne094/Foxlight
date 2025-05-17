using UnityEngine;

namespace Bap.State_Machine.Player
{
    public class Fall : BaseState
    {
        public Fall(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot)
        {
        }

        public override void Enter()
        {
            //Set Animator
            _ctx.IsFalling = true;
        }

        public override void Update()
        {
            RestrictFallingSpeed();
        }

        public override void Exit()
        {
            _ctx.IsFalling = false;
        }

        protected override void CheckTransition()
        {
            
        }
        
        private void RestrictFallingSpeed()
        {
            if (_ctx.Rb.linearVelocity.y < -_ctx.ControllerStatSo.MaxFallSpeed)
            {
                _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x, -_ctx.ControllerStatSo.MaxFallSpeed);
            }
        }
    }
}