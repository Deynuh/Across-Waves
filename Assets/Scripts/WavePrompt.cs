using UnityEngine;
using UnityEngine.UIElements;

public class WavePrompt : MonoBehaviour
{
    private VisualElement root;
    private VisualElement wavePromptElement;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Find your UI element by name/class
        wavePromptElement = root.Q("wave-prompt");

        // Initially hide it
        wavePromptElement.style.display = DisplayStyle.None;
    }

    void Update()
    {
        // Keep the B key for testing
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowWavePrompt();
        }
    }

    private void ShowWavePrompt()
    {
        if (wavePromptElement != null)
        {
            wavePromptElement.style.display = DisplayStyle.Flex;
            Debug.Log("Wave detected - UI Builder element shown!");
        }
    }

    public void HideWavePrompt()
    {
        if (wavePromptElement != null)
        {
            wavePromptElement.style.display = DisplayStyle.None;
            Debug.Log("UI Builder element hidden!");
        }
    }
    
    public bool IsWavePromptVisible()
    {
        if (wavePromptElement != null)
        {
            return wavePromptElement.style.display == DisplayStyle.Flex;
        }
        return false;
    }
    
}