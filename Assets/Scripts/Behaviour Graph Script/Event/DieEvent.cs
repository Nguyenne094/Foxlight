using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/DieEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "DieEvent", message: "[Self] die", category: "Events", id: "eb3155d4d9d1454ca86d1aa00f4184c8")]
public partial class DieEvent : EventChannelBase
{
    public delegate void DieEventEventHandler(GameObject Self);
    public event DieEventEventHandler Event; 

    public void SendEventMessage(GameObject Self)
    {
        Event?.Invoke(Self);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> SelfBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Self = SelfBlackboardVariable != null ? SelfBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Self);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        DieEventEventHandler del = (Self) =>
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
        Event += del as DieEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as DieEventEventHandler;
    }
}

