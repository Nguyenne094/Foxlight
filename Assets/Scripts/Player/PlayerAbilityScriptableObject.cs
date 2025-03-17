using DG.Tweening;
using UnityEngine;

namespace PlatformingGame.Controller
{
    public abstract class PlayerAbilityScriptableObject : ScriptableObject  // Changed from MonoBehaviour
    {
        [Header("General Configs")]
        public float CoolDownDuration;

        //No SerializeField here, as this is set in code.
        protected PlayerController _controller;
        public bool CanUse { get; protected set; } = true; // Initialize to true
        public bool Using { get; protected set; } = false; // Added public getter

        public virtual void Init(PlayerController controller)
        {
            _controller = controller;
            OnStart();
        }

        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public virtual void OnExit() { Restart(); }
        public abstract void Active();

        public abstract void Restart();

        protected void StartCoolDown()
        {
            CanUse = false;
            DOVirtual.DelayedCall(CoolDownDuration, () => CanUse = true);
        }
    }
}