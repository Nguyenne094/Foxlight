using UnityEngine;
using UnityEngine.Serialization;

namespace PlatformingGame.Controller
{
    [CreateAssetMenu(fileName = "Controller Status", menuName = "Player/Controller Status", order = 0)]
    public class ControllerStatSO : ScriptableObject
    {
        [Header("Movement Configs")]
        public float MoveSpeed = 3f;
        public float MoveSpeedWhenPushObject = 1.5f;
        public LayerMask ObjectMask;
        
        [Header("Jump Configs")]
        public float JumpHeight = 10;
        public float FallMultiplier = 2;
        public float NormalMultiplier = 1;
        [Min(0)] public float MaxFallSpeed = 20;
        public ContactFilter2D ContactFilter2D;

        [Header("Dash Configs")]
        [Min(0.1f)] public float DashForce = 5f;
        [Min(0.1f)] public float DashDuration = 0.1f;
        public float DashDelay = 0.5f;
    }
}