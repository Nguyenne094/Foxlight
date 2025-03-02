using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Dark_Bettle
{
    public class EdgeDetection : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ContactFilter2D _filter2D;

        private void Awake()
        {
            if (!_collider)
            {
                if (transform.parent)
                {
                    _collider = new BoxCollider2D();
                    var parent = GetComponentInParent<Transform>();
                    var parentCol = GetComponentInParent<Collider2D>();
                    var bottomRightColPos = parentCol.bounds.center +
                                            new Vector3(parentCol.bounds.size.x / 2, -parentCol.bounds.size.y / 2);
                    Debug.LogWarning("Reference to Collider is null and automatic create new BoxCollider2D in position: ");
                }
                else
                {
                    Debug.LogError("Please add this gameobject into a parent gameobject");
                }
            }
        }

        public bool IsFacingEdge()
        {
            bool value = _collider.Overlap(_filter2D, new List<Collider2D>()) == 0;
            return value;
        } 
    }
}