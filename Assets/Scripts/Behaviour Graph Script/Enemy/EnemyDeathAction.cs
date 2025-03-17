using System;
using Bap.Pool;
using DG.Tweening;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enemy Death", story: "On Enemy Death", category: "Enemy", id: "fc571bcbe60a39bf02feda0052c247d7")]
public partial class EnemyDeathAction : Action
{
    [SerializeReference] public BlackboardVariable<Bap.System.Health.Enemy> Enemy;

    private Tween spawnParticleTween;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var particle = EnemyDeathPSPool.Instance.MyPool.Get();
        particle.gameObject.transform.position = Enemy.Value.transform.position;
        spawnParticleTween = DOVirtual.DelayedCall(particle.main.startLifetime.constant, () => { });
        return Status.Success;
    }

    protected override void OnEnd()
    {
        spawnParticleTween.Kill();
        base.OnEnd();
    }
}

