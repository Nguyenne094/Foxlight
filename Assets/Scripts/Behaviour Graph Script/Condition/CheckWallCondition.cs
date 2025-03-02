using Enemy.Dark_Bettle;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Wall", story: "facing wall by [Detector]", category: "Conditions", id: "476ac33388b09fc329fee44bd6c3f772")]
public partial class CheckWallCondition : Condition
{
    [SerializeReference] public BlackboardVariable<WallDetection> Detector;

    public override bool IsTrue()
    {
        return Detector.Value.IsFacingWall();
    }
}
