using Bap.State_Machine;
using DG.Tweening;
using UnityEngine;

namespace PlatformingGame.Controller
{
    public abstract class PlayerAbilityScriptableObject : ScriptableObject  // Changed from MonoBehaviour
    {
        [Header("General Configs")]
        public float CoolDownDuration;

        //No SerializeField here, as this is set in code.
        protected PlayerContext _controller;
        public bool CanUse { get; protected set; } = true;
        public bool Using { get; protected set; } = false;

        public virtual void Init(PlayerContext controller)
        {
            _controller = controller;
        }

        protected abstract void Action();

        public void Active()
        {
            if (!Using && CanUse)
            {
                Action();
            }
        }

        public virtual void Restart(){}

        protected void StartCoolDown()
        {
            Using = true;
            CanUse = false;
            DOVirtual.DelayedCall(CoolDownDuration, () =>
            {
                CanUse = true;
                Using = false;
            });
        }
    }
}