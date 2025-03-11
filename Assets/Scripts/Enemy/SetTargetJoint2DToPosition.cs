using System;
using UnityEngine;

public class SetTargetJoint2DToPosition : MonoBehaviour
{
    [SerializeField] private TargetJoint2D _targetJoint2D;
    
    private void Awake()
    {
        if (!_targetJoint2D)
        {
            _targetJoint2D = GetComponent<TargetJoint2D>();
            if(!_targetJoint2D)
                Debug.LogError("Missing ref to TargetJoin2D");
        }
    }

    public void SetTargetPosition(Transform _target)
    {
        var pos = _target.position;
        _targetJoint2D.target = pos;
    }
}