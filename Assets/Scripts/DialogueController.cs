using UnityEngine;
using UnityEngine.UIElements;

public class DialogueController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button choice1;
    private Button choice2;

    private int index;  // where we are in the convo
    public string[] lines;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        choice1 = GetComponent<UIDocument>().rootVisualElement.Q("Choice1") as Button;
        choice1.RegisterCallback<ClickEvent>(OnChoice1Click);
        choice2 = GetComponent<UIDocument>().rootVisualElement.Q("Choice2") as Button;
        choice2.RegisterCallback<ClickEvent>(OnChoice2Click);
    }

    private void OnChoice1Click(ClickEvent e)
    {
        Debug.Log("Player chose Choice 1");
    }

    private void OnChoice2Click(ClickEvent e)
    {
        Debug.Log("Player chose Choice 2");
    }
    
    // Disables choice buttons when disabled, good practice
    private void OnDisable()
    {
        choice1.UnregisterCallback<ClickEvent>(OnChoice1Click);
        choice2.UnregisterCallback<ClickEvent>(OnChoice2Click);
    }
}
