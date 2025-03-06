using System;
using Unity.Behavior;
using UnityEngine;
using Utilities;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set TargetJoint2D Position", story: "Move Anchor through Positions with [MoveAnchorThroughList]", category: "Joint", id: "e15ae531bbfd0f67665e4bd316a1b673")]
public partial class MoveTargetJoint2DPositionThroughListAction : Action
{
    [SerializeReference] public BlackboardVariable<MoveTargetJoint2DThroughList> MoveAnchorThroughList;
    [SerializeReference] public BlackboardVariable<bool> Restart; 

    protected override Status OnStart()
    {
        if (MoveAnchorThroughList.Value == null)
        {
            Debug.LogError("Missing ref to MoveAnchorThroughList");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        MoveAnchorThroughList.Value.MoveThroughPosList();
        if (MoveAnchorThroughList.Value.ReachEndPos)
        {
            if (Restart)
            {
                MoveAnchorThroughList.Value.Restart();
            }
            else
            {
                return Status.Success;
            }
        }

        return Status.Running;
    }
}

