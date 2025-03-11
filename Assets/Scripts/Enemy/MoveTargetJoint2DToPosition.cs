using System.Linq;
using UnityEngine;

public class MoveTargetJoint2DToPosition : MonoBehaviour
{
    [SerializeField] private TargetJoint2D _targetJoint2D;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField, Min(0)] private float _threshold;


    private Vector2 _targetPos;
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

        _targetPos = _target.position;
    }

    public void Move()
    {
        if (Vector2.Distance(_targetJoint2D.target, _targetPos) <= _threshold && !ReachEndPos)
        {
            ReachEndPos = true;
        }
        _targetJoint2D.target = Vector3.MoveTowards(_targetJoint2D.target, _targetPos, _speed * Time.deltaTime);
    }

    public void Restart()
    {
        ReachEndPos = false;
    }
}