using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(BezierCurve))]
public class MoveTargetJoint2DThroughList : MonoBehaviour
{
    [SerializeField] private TargetJoint2D _targetJoint2D;
    [SerializeField] private BezierCurve _bezierCurve;
    [SerializeField] private float _speed;
    [SerializeField, Min(0)] private float _threshold;
    

    private int _currentIndex;
    private Vector2 _nextPos;
    private Vector2[] _posList;
    private bool _reachEndPos = false;
    private Rigidbody2D _rb;

    public bool ReachEndPos
    {
        get => _reachEndPos;
        private set => _reachEndPos = value;
    }

    private void Awake()
    {
        if (_targetJoint2D == null || _bezierCurve == null)
        {
            Debug.LogError("Missing required components.");
            enabled = false;
        }

        _rb = GetComponent<Rigidbody2D>();
        _bezierCurve = GetComponent<BezierCurve>();

        _posList = _bezierCurve.GetPositionArray();
        
        _currentIndex = 0;
        _nextPos = _posList[_currentIndex];
    }

    public void MoveThroughPosList()
    {
        if (Vector2.Distance(_targetJoint2D.target, _nextPos) <= _threshold)
        {
            if (_currentIndex == _posList.Length-1)
            {
                ReachEndPos = true;
               _posList = _posList.Reverse().ToArray();//Because of Reverse of LinQ return a list or array without changing the original
            }
            else
            {
                _currentIndex++;
                _nextPos = _posList[_currentIndex];
            }
        }
        else
        {
            _targetJoint2D.target = Vector3.MoveTowards(_targetJoint2D.target, _nextPos, _speed * Time.deltaTime);
        }
    }

    public void Restart()
    {
        _currentIndex = 0;
        _nextPos = _posList[_currentIndex];
        ReachEndPos = false;
    }
}
