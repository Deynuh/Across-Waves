using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
    
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public string targetNodeID;
    }

    [System.Serializable]
    public class DialogueNode
    {
        public string nodeID;
        [TextArea(3,10)]
        public string dialogueText;
        public List<DialogueChoice> choices;
        public string nextNodeID;
    }
}
