using System;
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
                Debug.LogError(gameObject.name + ": Missing ref to Target to detect");
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