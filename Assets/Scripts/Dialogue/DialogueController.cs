using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private string startingNodeID = "start";
    
    private Button choice1;
    private Button choice2;
    private Label dialogueText;

    private DialogueData.DialogueNode currentNode;
    private List<DialogueData.DialogueNode> allNodes;
    
    private void Awake()
    {
        dialogueText = GetComponent<UIDocument>().rootVisualElement.Q("Text") as Label;
        
        dialogueText.RegisterCallback<ClickEvent>(OnDialogueClick);
        
        choice1 = GetComponent<UIDocument>().rootVisualElement.Q("Choice1") as Button;
        choice1.RegisterCallback<ClickEvent>(OnChoice1Click);
        choice2 = GetComponent<UIDocument>().rootVisualElement.Q("Choice2") as Button;
        choice2.RegisterCallback<ClickEvent>(OnChoice2Click);

        allNodes = dialogueData.nodes;
    }
    
    private void OnDialogueClick(ClickEvent e)
    {
        // Only continue if there are no choices
        if (currentNode.choices == null || currentNode.choices.Count == 0)
        {
            ContinueDialogue();
        }
    }
    
    private void ContinueDialogue()
    {
        if (!string.IsNullOrEmpty(currentNode.nextNodeID))
        {
            NavigateToNode(currentNode.nextNodeID);
        }
        else
        {
            Debug.Log("Dialogue ended");
        }
    }

    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        currentNode = allNodes.FirstOrDefault(node => node.nodeID == startingNodeID);
        if (currentNode != null)
        {
            DisplayCurrentNode();
        }
        else
        {
            Debug.LogError("Starting node not found: " + startingNodeID);
        }
    }
    
    private void DisplayCurrentNode()
    {
        dialogueText.text = currentNode.dialogueText;
        
        // handle choices
        if (currentNode.choices != null && currentNode.choices.Count == 2)
        {
            choice1.style.display = DisplayStyle.Flex;
            choice1.text = currentNode.choices[0].choiceText;
            choice2.style.display = DisplayStyle.Flex;
            choice2.text = currentNode.choices[1].choiceText;
        }
        // no choices, hide buttons
        else
        {
            
            choice1.style.display = DisplayStyle.None;
            choice2.style.display = DisplayStyle.None;
        }
    }
    
    private void NavigateToNode(string nodeID)
    {
        currentNode = allNodes.FirstOrDefault(node => node.nodeID == nodeID);
        if (currentNode != null)
        {
            DisplayCurrentNode();
        }
        else
        {
            Debug.LogWarning($"Node with ID '{nodeID}' not found!");
        }
    }
    
    private void OnChoice1Click(ClickEvent e) 
    {
        if (currentNode.choices != null && currentNode.choices.Count > 0)
        {
            NavigateToNode(currentNode.choices[0].targetNodeID);
        }
    }

    private void OnChoice2Click(ClickEvent e)
    {
        if (currentNode.choices != null && currentNode.choices.Count > 1)
        {
            NavigateToNode(currentNode.choices[1].targetNodeID);
        }
    }
    
    // Disables choice buttons when disabled, good practice
    private void OnDisable()
    {
        choice1.UnregisterCallback<ClickEvent>(OnChoice1Click);
        choice2.UnregisterCallback<ClickEvent>(OnChoice2Click);
    }
}
