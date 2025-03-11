using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _point2;
        [SerializeField] private Transform _point3;

        private void OnDrawGizmos()
        {
            if (_point1 && _point2 && _point3)
            { 
                Vector3[] positionList = new Vector3[11];
                for (int i = 0; i <= 10; i++)
                {
                    float percent = i / 10f;
                    positionList[i] = GetPosition(percent);
                }
                Gizmos.DrawLineStrip(positionList, false);
            }
        }

        /// <summary>
        /// Return an array containing 10 position elements lying on the bezier curve
        /// </summary>
        public Vector2[] GetPositionArray()
        {
            Vector2[] posArray = new Vector2[11];

            if (_point1 && _point2 && _point3)
            {
                for (int i = 0; i <= 10; i++)
                {
                    float percent = i / 10f;
                    posArray[i] = GetPosition(percent);
                }
            }
            return posArray;
        }

        private Vector2 GetPosition(float percent)
        {
            //P = (1-t).P1 + t.P2
            Vector2 b1 = (1 - percent) * _point1.position + percent * _point2.position;
            Vector2 b2 = (1 - percent) * _point2.position + percent * _point3.position;
            Vector2 b = (1 - percent) * b1 + percent * b2;

            return b;
        }
    }
}