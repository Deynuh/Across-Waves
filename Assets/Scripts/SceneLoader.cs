using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button nextSceneButton;
    private Label sceneInfoLabel;

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null)
        {
            SetupUI();
        }
    }

    private void SetupUI()
    {
        var root = uiDocument.rootVisualElement;
        
        nextSceneButton = root.Q<Button>("NextSceneButton");
        sceneInfoLabel = root.Q<Label>("SceneInfoLabel");

        if (nextSceneButton != null)
        {
            nextSceneButton.clicked += LoadNextScene;
        }
        if (sceneInfoLabel != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            sceneInfoLabel.text = $"Scene: {sceneName} (Index: {sceneIndex})";
        }
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("Already on last scene.");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}