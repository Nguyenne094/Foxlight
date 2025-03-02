using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FlipDirectionX", story: "[Transform] flips direction X", category: "Action", id: "6bfd88eee5470013f28ecaafd3cec042")]
public partial class FlipDirectionXAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Transform;
    [Tooltip("The nornalized vector2 that Transform want to flip to")]
    [SerializeReference] public BlackboardVariable<Vector2> FacingDirectionVector2;

    protected override Status OnStart()
    {
        if (Transform.Value == null)
        {
            LogFailure($"Missing Transform.");
            return Status.Failure;
        }

        FlipDirection();
        
        return Status.Success;
    }

    private void FlipDirection()
    {
        if (Math.Sign(Transform.Value.localScale.x) < 0)
        {
            FacingDirectionVector2.Value = Vector2.right;
        }
        else
        {
            FacingDirectionVector2.Value = Vector2.left;
        }
        Transform.Value.localScale = new Vector3(
            Transform.Value.localScale.x * -1, 
            Transform.Value.localScale.y,
            Transform.Value.localScale.z);
    }
}

