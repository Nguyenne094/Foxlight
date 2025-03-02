using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Log Message With Name", story: "Log [Message] With Name [Self]", category: "Action/Debug", id: "fa8de4a72e4e0bb8d46bcf64142b7989")]
public partial class LogMessageWithName : Action
{
    [SerializeReference] public BlackboardVariable<string> Message;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<LogType> LogLevel = new BlackboardVariable<LogType>(LogType.Info);
    
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    protected override Status OnStart()
    {
        if (Message == null)
        {
            return Status.Failure;
        }

        switch (LogLevel.Value)
        {
            case LogType.Info:
                Debug.Log(Self.Value.name + " logs: " + Message.Value, GameObject);
                break;
            case LogType.Warning:
                Debug.LogWarning(Self.Value.name + " warns: " + Message.Value, GameObject);
                break;
            case LogType.Error:
                Debug.LogError(Self.Value.name + " error: " + Message.Value, GameObject);
                break;
        }

        return Status.Success;
    }
}

