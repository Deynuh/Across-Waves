using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class CallManager : MonoBehaviour
{
    private Button acceptButton;

    private VisualElement callScreen;

    private VisualElement incomingCall;

    [SerializeField] private GameObject dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceptButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Accept");
        callScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("CallScreen");
        incomingCall = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("IncomingCall");
        
        acceptButton.clicked += OnAcceptButtonClicked;
    }

    void OnAcceptButtonClicked()
    {
        incomingCall.style.display = DisplayStyle.None;
        callScreen.style.display = DisplayStyle.Flex;

        StartCoroutine(EnableDialogueAfterDelay());

    }

    private IEnumerator EnableDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        dialogue.SetActive(true);
    }
    
    void OnDestroy()
    {
        // Unregister to prevent memory leaks
        if (acceptButton != null)
        {
            acceptButton.clicked -= OnAcceptButtonClicked;
        }
    }
}
