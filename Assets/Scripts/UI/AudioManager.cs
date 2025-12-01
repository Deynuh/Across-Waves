using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Set up global click detection when scenes load
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        // Set up for current scene if already loaded
        SetupGlobalClickListener();
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Small delay to ensure UI is fully loaded
        Invoke(nameof(SetupGlobalClickListener), 0.1f);
    }

    private void SetupGlobalClickListener()
    {
        // Find all UIDocuments in the scene and add click listeners
        UIDocument[] uiDocuments = FindObjectsByType<UIDocument>(FindObjectsSortMode.None);

        foreach (UIDocument uiDoc in uiDocuments)
        {
            if (uiDoc.rootVisualElement != null)
            {
                uiDoc.rootVisualElement.RegisterCallback<ClickEvent>(OnAnyUIClick, TrickleDown.TrickleDown);
            }
        }
    }

    private void OnAnyUIClick(ClickEvent evt)
    {
        PlayClickSound();
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}