using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public KeyCode testKey = KeyCode.Space;
    [SerializeField] private UIDocument uiDocument;
    private float fadeDuration = 2f;

    private Button startGameButton;
    private GameObject fade;
    private VisualElement fadeElement;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Transform fadeTransform = transform.Find("Fade");
        fade = fadeTransform.gameObject;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            startGameButton = root.Q<Button>("StartGame");
            startGameButton.clicked += OnStartGameClicked;
        }

        UIDocument fadeUIDoc = fade.GetComponent<UIDocument>();
        fadeElement = fadeUIDoc.rootVisualElement.Q("BlackScreen");
        fade.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fade.SetActive(true);
        UIDocument fadeUIDoc = fade.GetComponent<UIDocument>();
        if (fadeUIDoc != null && fadeUIDoc.rootVisualElement != null)
        {
            fadeElement = fadeUIDoc.rootVisualElement.Q("BlackScreen");

        }

        // Always fade in when a new scene loads (except first scene which uses Start())
        if (fadeElement != null && scene.buildIndex > 0)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float startOpacity = 1f;
        float endOpacity = 0f;

        fade.SetActive(true);
        fadeElement.pickingMode = PickingMode.Position;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentOpacity = Mathf.Lerp(startOpacity, endOpacity, elapsedTime / fadeDuration);
            fadeElement.style.opacity = currentOpacity;
            yield return null;
        }

        fadeElement.style.opacity = endOpacity;
        fadeElement.pickingMode = PickingMode.Ignore;
        fade.SetActive(false);
    }

    private void OnStartGameClicked()
    {
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene(1);
            fade.SetActive(true);
            fadeElement.pickingMode = PickingMode.Position;
        }
        else
        {
            Debug.LogError("No game scene found at index 1!");
        }
    }

    public IEnumerator FadeOutAndLoadNextScene()
    {
        fade.SetActive(true);

        UIDocument fadeUIDoc = fade.GetComponent<UIDocument>();
        if (fadeUIDoc != null && fadeUIDoc.rootVisualElement != null)
        {
            fadeElement = fadeUIDoc.rootVisualElement.Q("BlackScreen");
        }
        fadeElement.pickingMode = PickingMode.Position;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentIndex + 1;

        float elapsedTime = 0f;
        float startOpacity = 0f;
        float endOpacity = 1f;
        Debug.Log("Starting fade out from " + startOpacity + " to " + endOpacity);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentOpacity = Mathf.Lerp(startOpacity, endOpacity, elapsedTime / fadeDuration);
            fadeElement.style.opacity = currentOpacity;
            yield return null;
        }

        fadeElement.style.opacity = endOpacity;
        Debug.Log("Fade out complete, loading next scene");

        // Load scene after fade completes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError($"Scene index {nextSceneIndex} not found!");
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeOutAndLoadNextScene());
    }

    // Call this when the action finishes (e.g. animation event, coroutine end, trigger, etc.)
    public void OnWaveFinished()
    {
        StartCoroutine(FadeOutAndLoadNextScene());
        Debug.Log("Loading next scene from wave finished");
    }

    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            StartCoroutine(FadeOutAndLoadNextScene());
            Debug.Log("Loading next scene from calling (test key)");
        }
    }

    private void OnDestroy()
    {
        // Unregister button to prevent memory leaks
        if (startGameButton != null)
        {
            startGameButton.clicked -= OnStartGameClicked;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}