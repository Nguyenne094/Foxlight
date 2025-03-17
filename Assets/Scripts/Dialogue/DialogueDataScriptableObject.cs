using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue_Data", menuName = "Dilogue", order = 0)]
    public class DialogueDataScriptableObject : ScriptableObject
    {
        public enum TalkToWho {Player, Other}
        public TalkToWho TalkTo;
        public List<string> Speech;
    }
}