using UnityEngine;

namespace Bap.State_Machine.Player
{
    public class OnAir : BaseState
    {
        public OnAir(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot) {}

        public override void Enter()
        {
            InitializeSubState();
        }

        public override void Update(){}

        public override void Exit(){}

        protected override void CheckTransition()
        {
            if (_ctx.OnGround)
            {
                _ctx.TransitionRootState(this, _factory.GetState<Grounded>());
            }
            else if (_ctx.IsRolling)
            {
                _ctx.TransitionRootState(this, _factory.GetState<Roll>());
            }
        }

        public override void InitializeSubState()
        {
            var state = _ctx.IsJumping 
                ? _factory.GetState<Jump>() 
                :  _factory.GetState<Fall>();
            
            SetSubState(state);
            state.Enter();
            _ctx.SetCurrentSubState(state);
        }
    }
}