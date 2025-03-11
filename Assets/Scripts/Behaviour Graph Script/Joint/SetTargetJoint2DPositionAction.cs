using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set TargetJoint2D Position", story: "Set [TargetJoint2DPosition] to [Position]", category: "Action", id: "41c68bfdb83caacf4cf83719f03ff2df")]
public partial class SetTargetJoint2DPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<SetTargetJoint2DToPosition> TargetJoint2DPosition;
    [SerializeReference] public BlackboardVariable<Transform> Position;
    protected override Status OnStart()
    {
        TargetJoint2DPosition.Value.SetTargetPosition(Position);
        return Status.Success;
    }
}

