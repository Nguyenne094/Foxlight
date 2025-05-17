using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Bap.State_Machine.Player
{
    public class Idle : BaseState
    {
        public Idle(PlayerContext ctx, StateFactory factory, bool isRoot) : base(ctx, factory, isRoot) {}

        public override void Enter()
        {
            _ctx.Anim.SetBool(PlayerAnimationString.IsWalking, false);
        }

        public override void Update(){}

        public override void Exit(){}

        protected override void CheckTransition()
        {
            if (_ctx.IsMoving)
            {
                _ctx.TransitionSubState(this, _factory.GetState<Walk>());
            }
        }

        public override void InitializeSubState(){}
    }
}