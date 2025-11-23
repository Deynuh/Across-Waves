using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public KeyCode testKey = KeyCode.Space;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.Log("Already on last scene.");
    }
    
    // Call this when the action finishes (e.g. animation event, coroutine end, trigger, etc.)
    public void OnWaveFinished()
    {
        Instance.LoadNextScene();
        Debug.Log("Loading next scene from wave finished");
    }

    // Example: invoke after a timed action
    public void StartTimedAction(float seconds)
    {
        StartCoroutine(TimedAction(seconds));
    }
    
    private System.Collections.IEnumerator TimedAction(float s)
    {
        yield return new WaitForSeconds(s);
        Instance.LoadNextScene();
        Debug.Log("Loading next scene from timed action");
    }

    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            Instance.LoadNextScene();
            Debug.Log("Loading next scene from calling (test key)");
        }
    }
}