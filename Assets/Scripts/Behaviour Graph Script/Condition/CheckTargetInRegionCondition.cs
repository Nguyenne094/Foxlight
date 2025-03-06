using Enemy;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Target In Region", story: "Detect [TargetInRegion]", category: "Conditions", id: "3b441cb993a406f982fdb4c8377dc0b9")]
public partial class CheckTargetInRegionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<DetectTargetInRegion> TargetInRegion;

    public override bool IsTrue()
    {
        return TargetInRegion.Value.DetectedTarget();
    }
}
