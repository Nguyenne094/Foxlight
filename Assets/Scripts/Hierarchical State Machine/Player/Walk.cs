using NotImplementedException = System.NotImplementedException;
using UnityEngine;

namespace Bap.State_Machine.Player
{
    public class Walk : BaseState
    {
        public Walk(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot) {}

        public override void Enter() {
            _ctx.Anim.SetBool(PlayerAnimationString.IsWalking, true);
        }
        
        public override void Update(){}
        public override void Exit() => _ctx.Anim.SetBool(PlayerAnimationString.IsWalking, false);
        protected override void CheckTransition()
        {
            if (!_ctx.IsMoving)
            {
                _ctx.TransitionSubState(this, _factory.GetState<Idle>());
            }
        }

        public override void InitializeSubState() {}
    }
}