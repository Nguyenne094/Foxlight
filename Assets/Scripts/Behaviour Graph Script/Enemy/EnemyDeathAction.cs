using System;
using Bap.Pool;
using DG.Tweening;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enemy Death", story: "On Enemy Death", category: "Action", id: "fc571bcbe60a39bf02feda0052c247d7")]
public partial class EnemyDeathAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    
    protected override Status OnStart()
    {
        var particle = EnemyDeathPSPool.Instance.MyPool.Get();
        particle.gameObject.transform.position = Self.Value.transform.position;
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
}

