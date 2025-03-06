using System;
using Enemy;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Target Joint 2D to a position", story: "Move Target Anchor to a position with [MoveAnchorToPosition]", category: "Action", id: "02c0309d6096510c5d0263cf699cce5e")]
public partial class MoveTargetJoint2DToAPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<MoveTargetJoint2DToPosition> MoveAnchorToPosition;
    [SerializeReference] public BlackboardVariable<bool> Restart;

    protected override Status OnStart()
    {
        if (MoveAnchorToPosition.Value == null)
        {
            Debug.LogError("Missing ref to MoveAnchorThroughList");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (MoveAnchorToPosition.Value.ReachEndPos)
        {
            if (Restart)
            {
                MoveAnchorToPosition.Value.Restart();
            }
            else
            {
                return Status.Success;
            }
        }
        else
        {
            MoveAnchorToPosition.Value.Move();
        }

        return Status.Running;
    }
}

