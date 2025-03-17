using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlatformingGame.Controller
{
    [CreateAssetMenu(fileName = "Controller Status", menuName = "Player/Controller Status", order = 0)]
    public class ControllerStatSO : ScriptableObject
    {
        
        [Header("Movement Configs")]
        public float MoveSpeed = 3f;
        
        [Header("Jump Configs")]
        public float JumpHeight = 10;
        public float JumpDuration = 0.4f;
        [Min(0)] public float MaxFallSpeed = 20;
        public ContactFilter2D GroundFilter;
        public float GroundCheckDistance = 0.05f;

        [Header("Abilities")] 
        public PlayerAbilityScriptableObject Roll;

        public void Init(PlayerController controller)
        {
            if(Roll != null)
                Roll.Init(controller);
        }
    }
}