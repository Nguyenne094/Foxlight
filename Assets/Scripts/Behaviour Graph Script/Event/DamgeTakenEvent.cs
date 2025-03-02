using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TakenDamageEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TakenDamageEvent", message: "[Self] Takes Damage", category: "Events", id: "c5b130f1fa1fe8dffebd6a0258e5ad63")]
public partial class DamgeTakenEvent : EventChannelBase
{
    public delegate void DamgeTakenEventEventHandler(GameObject Self, int Damage);
    public event DamgeTakenEventEventHandler Event; 

    public void SendEventMessage(GameObject Self, int Damage)
    {
        Event?.Invoke(Self, Damage);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> SelfBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Self = SelfBlackboardVariable != null ? SelfBlackboardVariable.Value : default(GameObject);
        BlackboardVariable<int> DamageBlackboardVariable = messageData[0] as BlackboardVariable<int>;
        var Damage = DamageBlackboardVariable != null ? DamageBlackboardVariable.Value : default(int);
        
        Event?.Invoke(Self, Damage);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        DamgeTakenEventEventHandler del = (Self, Damage) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Self;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as DamgeTakenEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as DamgeTakenEventEventHandler;
    }
}

