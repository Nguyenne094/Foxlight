using Enemy.Dark_Bettle;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckEdge", story: "Check is facing an edge by [Detector]", category: "Conditions", id: "a0651716f8983035d1967820ee7b3b96")]
public partial class CheckEdgeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EdgeDetection> Detector;

    public override bool IsTrue()
    {
        return Detector.Value.IsFacingEdge();
    }

    public override void OnStart()
    {
        if (!Detector.Value)
        {
            Debug.LogError("Error: Missing reference to Detector");
        }
    }
}
