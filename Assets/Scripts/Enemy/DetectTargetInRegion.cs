using System;
using Bap.System.Health;
using UnityEngine;

namespace Enemy
{
    public class DetectTargetInRegion : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Collider2D[] _regions;

        private void Awake()
        {
            if (!_target)
            {
                _target = FindFirstObjectByType<PlayerHealth>().transform;
                if (!_target)
                {
                    Debug.LogError("Player is not in current scene!!!");
                }
            }
        }

        public bool DetectedTarget()
        {
            foreach (var region in _regions)
            {
                if (region.IsTouching(_target.GetComponent<Collider2D>()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}