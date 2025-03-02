using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy.Dark_Bettle
{
    public class WallDetection : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _detectDistance;
        
        [SerializeField] private ContactFilter2D _filter2D;
        

        private void Awake()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
                if(_collider == null) Debug.LogError("Miss ref to Collider");
            }
        }

        public bool IsFacingWall()
        {
            List<RaycastHit2D> rightHits = new();
            List<RaycastHit2D> leftHits = new();
            bool isFacingWallRight = Physics2D.Raycast(_collider.bounds.center, new Vector2(transform.localScale.x, 0), _filter2D, rightHits, _detectDistance) > 0;
            bool isFacingWallLeft = Physics2D.Raycast(_collider.bounds.center, new Vector2(transform.localScale.x, 0), _filter2D, leftHits, _detectDistance) > 0;

            bool isFacingWall = isFacingWallRight || isFacingWallLeft;
            
            return isFacingWall;
        }

        private void OnDrawGizmosSelected()
        {
            if (_collider)
            {
                Gizmos.DrawLine(
                    _collider.bounds.center, 
                    _collider.bounds.center + (Vector3)Vector2.right * _detectDistance);
                Gizmos.DrawLine(
                    _collider.bounds.center, 
                    _collider.bounds.center + (Vector3)Vector2.left * _detectDistance);
            }
        }
    }
}