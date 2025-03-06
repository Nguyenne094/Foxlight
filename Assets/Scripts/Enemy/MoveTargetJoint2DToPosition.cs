using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class MoveTargetJoint2DToPosition : MonoBehaviour
    {
        [SerializeField] private TargetJoint2D _targetJoint2D;
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        [SerializeField] private float _speed;
        [SerializeField, Min(0)] private float _threshold;


        private Vector2 _startPos;
        private Vector2 _endPos;
        private Vector2 _currentPos;
        private bool _reachEndPos = false;

        public bool ReachEndPos
        {
            get => _reachEndPos;
            private set => _reachEndPos = value;
        }

        private void Awake()
        {
            if (_targetJoint2D == null)
            {
                Debug.LogError("Missing required components.");
                enabled = false;
            }

            _startPos = _start.position;
            _endPos = _end.position;
            _currentPos = _startPos;
        }

        public void Move()
        {
            if (Vector2.Distance(_targetJoint2D.target, _currentPos) <= _threshold)
            {
                if (_currentPos == _endPos)
                {
                    ReachEndPos = true;
                }
                else
                {
                    _currentPos = _endPos;
                }
            }
            _targetJoint2D.target = Vector3.MoveTowards(_targetJoint2D.target, _currentPos, _speed * Time.deltaTime);
        }

        public void Restart()
        {
            Vector2 temp = _startPos;
            _startPos = _endPos;
            _endPos = temp;
            _currentPos = _startPos;
            ReachEndPos = false;
        }
    }
}