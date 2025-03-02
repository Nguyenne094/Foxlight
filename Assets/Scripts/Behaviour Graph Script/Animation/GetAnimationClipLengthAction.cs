using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get AnimationClip Length", story: "Get Current AnimationCLip Length on [Self] out [Variable]", category: "Action", id: "06bad134db786de20865286625244b44")]
public partial class GetAnimationClipLengthAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Variable;

    protected override Status OnStart()
    {
        Variable.Value = Self.Value.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
        
        return Status.Success;
    }
}

