using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Position", story: "[Self] move to [TargetPosition] with Speed [Speed]", category: "Action/Physics", id: "3bf9d64bf720ce50632b6525e502facc")]
public partial class MovePositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> TargetPosition;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> Threshold;

    private Rigidbody2D _rb;
    
    protected override Status OnStart()
    {
        if (TargetPosition.Value == null || Speed == null || Threshold == null)
        {
            Debug.LogError("Missing required Blackboard values");
            return Status.Failure;
        }
        
        _rb = Self.Value.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on Self");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var dir = (TargetPosition.Value.position - Self.Value.transform.position).normalized;
        _rb.MovePosition(Self.Value.transform.position + dir * Speed * Time.deltaTime);

        if (Vector3.Distance(TargetPosition.Value.position, Self.Value.transform.position) < Threshold.Value)
        {
            Debug.Log("Success");
            return Status.Success;
        }
        
        return Status.Running;
    }
}

