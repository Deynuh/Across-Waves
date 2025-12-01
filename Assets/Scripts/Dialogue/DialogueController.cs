using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private string startingNodeID = "start";
    [SerializeField] private string nodeForWave;
    [SerializeField] private string lagID;
    [SerializeField] private GameObject wavePrompt;
    [SerializeField] private WavePrompt wavePromptScript;
    [SerializeField] private GameObject screen;
    [SerializeField] private bool shouldShowGameInvite = false;
    [SerializeField] private bool shouldShowConnectionGame = false;
    [SerializeField] private GameObject connectionGame;
    [SerializeField] private string endingNodeID = "end";

    private Button choice1;
    private Button choice2;
    private Label dialogueText;
    private VisualElement dialogueContainer;

    private DialogueData.DialogueNode currentNode;
    private List<DialogueData.DialogueNode> allNodes;

    private ArtManager artManager;
    private bool lagging = false;


    private void Awake()
    {
        wavePromptScript = wavePrompt.GetComponent<WavePrompt>();
        artManager = FindFirstObjectByType<ArtManager>();

        dialogueContainer = GetComponent<UIDocument>().rootVisualElement.Q("Box") as VisualElement;
        dialogueText = GetComponent<UIDocument>().rootVisualElement.Q("Text") as Label;
        dialogueContainer.RegisterCallback<ClickEvent>(OnDialogueClick);

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
        if (lagging)
        {
            artManager.SelectSprites(2);
        }

        // check if node should trigger the wave prompt
        if (currentNode.nodeID == nodeForWave)
        {
            wavePrompt.SetActive(true);
            artManager.SelectSprites(1);
            StartCoroutine(WaitForWaveCompletion(currentNode.nextNodeID));
        }
        // display current node and then after a small 3 second wait, show game invite
        else if (string.IsNullOrEmpty(currentNode.nextNodeID) && shouldShowGameInvite || shouldShowConnectionGame)
        {
            Debug.Log("Reached last node, preparing to show game");
            StartCoroutine(ShowGameInviteAfterDelay(3f));
        }
        else if (string.IsNullOrEmpty(currentNode.nextNodeID))
        {
            lagging = false;
            Debug.Log("End of dialogue, loading next scene");
            SceneLoader.Instance.LoadNextScene();
        }
        // go to next node
        else
        {

            currentNode = allNodes.FirstOrDefault(node => node.nodeID == currentNode.nextNodeID);
            if (currentNode != null)
            {
                DisplayCurrentNode();
            }
            else
            {
                Debug.LogError("Node not found: " + currentNode.nextNodeID);
            }
        }
    }

    private IEnumerator ShowGameInviteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Showing game after delay");

        if (shouldShowGameInvite)
        {
            var gameInviteElement = screen.GetComponent<UIDocument>().rootVisualElement.Q("GameInvite");
            if (gameInviteElement != null)
            {
                gameInviteElement.style.display = DisplayStyle.Flex;
            }
        }

        if (shouldShowConnectionGame && connectionGame != null)
        {
            connectionGame.SetActive(true);
        }

        dialogueContainer.style.display = DisplayStyle.None;
    }

    private IEnumerator WaitForWaveCompletion(string nextNodeID)
    {
        dialogueContainer.style.display = DisplayStyle.None;

        while (!wavePromptScript.IsWaveComplete())
        {
            yield return null;
        }

        Debug.Log("Wave complete, continuing dialogue");
        dialogueContainer.style.display = DisplayStyle.Flex;
        wavePrompt.SetActive(false);
        if (!lagging)
        {
            artManager.SelectSprites(0);
        }


        currentNode = allNodes.FirstOrDefault(node => node.nodeID == nextNodeID);
        if (currentNode != null)
        {
            DisplayCurrentNode();
        }
        else if (string.IsNullOrEmpty(nextNodeID) || nextNodeID == endingNodeID)
        {
            Debug.Log("End of dialogue and final wave in scene, handling scene transition.");
            SceneLoader.Instance.OnWaveFinished();
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
        if (currentNode.nodeID == lagID)
        {
            artManager.SelectSprites(2);
            lagging = true;
        }

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

    private void OnChoice1Click(ClickEvent e)
    {
        if (currentNode.choices != null && currentNode.choices.Count > 0)
        {
            string targetNodeID = currentNode.choices[0].targetNodeID;
            currentNode = allNodes.FirstOrDefault(node => node.nodeID == targetNodeID);

            if (currentNode != null)
            {
                DisplayCurrentNode();
            }
            else
            {
                Debug.LogError("Node not found: " + targetNodeID);
            }
        }
    }

    private void OnChoice2Click(ClickEvent e)
    {
        if (currentNode.choices != null && currentNode.choices.Count > 1)
        {
            string targetNodeID = currentNode.choices[1].targetNodeID;
            currentNode = allNodes.FirstOrDefault(node => node.nodeID == targetNodeID);

            if (currentNode != null)
            {
                DisplayCurrentNode();
            }
            else
            {
                Debug.LogError("Node not found: " + targetNodeID);
            }
        }
    }

    // Disables choice buttons when disabled, good practice
    private void OnDisable()
    {
        choice1.UnregisterCallback<ClickEvent>(OnChoice1Click);
        choice2.UnregisterCallback<ClickEvent>(OnChoice2Click);
    }
}
