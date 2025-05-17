using UnityEngine;

namespace Bap.State_Machine.Player
{
    public class Roll : BaseState
    {
        public Roll(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot) {}

        public override void Enter()
        {
            _ctx.ControllerStatSo.Roll.Active();
            _ctx.IsRolling = true;
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            _ctx.IsRolling = false;
        }

        protected override void CheckTransition()
        {
            if (!_ctx.ControllerStatSo.Roll.Using)
            {
                if (_ctx.OnGround)
                {
                    _ctx.TransitionRootState(this, _factory.GetState<Grounded>());
                }
                else
                {
                    _ctx.TransitionRootState(this, _factory.GetState<Jump>());
                }
            }
        }
        
        public override void InitializeSubState(){}
    }
}