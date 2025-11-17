using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CountdownManager : MonoBehaviour
{
    [SerializeField] private string countdownNum = "100";

    private Label labelNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        labelNumber = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Number");
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateNumber();
    }

    private void UpdateNumber()
    {
        labelNumber.text = countdownNum;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateNumber();
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //to prevent memory leak
    }

}
