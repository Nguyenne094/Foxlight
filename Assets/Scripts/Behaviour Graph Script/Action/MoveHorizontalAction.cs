using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Horizontal", story: "[Self] move along horizontal axis with [Speed]", category: "Action", id: "fc78fb5e00f5940cad9513f9966f05f9")]
public partial class MoveHorizontalAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<Vector2> FacingDirectionVector2;

    protected override Status OnStart()
    {
        if (!Self.Value)
        {
            Debug.LogError("Error: Missing reference to Self");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Self.Value.transform.Translate(FacingDirectionVector2.Value * Speed * Time.deltaTime);
        
        return Status.Running;
    }
}

