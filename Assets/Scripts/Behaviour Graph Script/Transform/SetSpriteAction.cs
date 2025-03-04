using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Sprite", story: "Set [Self] Sprite to [Sprite]", category: "Action/Transform", id: "d855e88af13ddcc10d4e48ac2aadc17a")]
public partial class SetSpriteAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Sprite> Sprite;

    protected override Status OnStart()
    {
        var sprite = Self.Value.transform.GetComponent<SpriteRenderer>();

        if (sprite)
        {
            sprite.sprite = Sprite;
        }
        else
        {
            Debug.LogError("Miss SpriteRenderer in " + Self.Value.name);
        }
        
        return Status.Success;
    }
}

