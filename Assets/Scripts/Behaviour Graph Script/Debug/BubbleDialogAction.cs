using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using TMPro;
using UnityEngine.UI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Bubble Dialog", story: "[Self] says [Message] in Bubble Dialog", category: "Action/Debug", id: "b5642b08962d93599d726530c68f5578")]
public partial class BubbleDialogAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<string> Message;
    [SerializeReference] public BlackboardVariable<float> displayDuration;
    [SerializeReference] public BlackboardVariable<LogLevel> logLevel = new BlackboardVariable<LogLevel>(LogLevel.Log);

    private float timer;
    private GameObject bubbleDialog;

    protected override Status OnStart()
    {
        timer = displayDuration.Value == 0 ? 1 : displayDuration.Value;
        ShowBubbleDialog(Self.Value, Message.Value, logLevel.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            HideBubbleDialog(Self.Value);
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        HideBubbleDialog(Self.Value);
    }

    private void ShowBubbleDialog(GameObject self, string message, LogLevel level)
    {
        // Create a new Canvas
        bubbleDialog = new GameObject("BubbleDialog");
        bubbleDialog.transform.parent = Self.Value.transform;
        Canvas canvas = bubbleDialog.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        CanvasScaler canvasScaler = bubbleDialog.AddComponent<CanvasScaler>();
        canvasScaler.dynamicPixelsPerUnit = 10;
        bubbleDialog.AddComponent<GraphicRaycaster>();

        // Create a TextMeshProUGUI element
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(bubbleDialog.transform);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = message;
        text.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
        text.fontSize = 0.7f;
        text.alignment = TextAlignmentOptions.Center;

        // Set text color based on log level
        switch (level)
        {
            case LogLevel.Log:
                text.color = Color.white;
                break;
            case LogLevel.Warn:
                text.color = Color.yellow;
                break;
            case LogLevel.Error:
                text.color = Color.red;
                break;
        }

        // Position the Canvas above the self GameObject
        RectTransform rectTransform = bubbleDialog.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);
        rectTransform.position = self.transform.position + new Vector3(0, 2, 0);
    }

    private void HideBubbleDialog(GameObject self)
    {
        if (bubbleDialog != null)
        {
            GameObject.Destroy(bubbleDialog);
        }
    }

    public enum LogLevel
    {
        Log,
        Warn,
        Error
    }
}