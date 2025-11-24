using UnityEngine;
using UnityEngine.UIElements;

public class GameInvite : MonoBehaviour
{
    private Button gameAcceptButton;
    private VisualElement gameInvite;
    [SerializeField] private GameObject miniGame;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameAcceptButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("GameAccept");
        gameInvite = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("GameInvite");
        
        gameAcceptButton.clicked += OnAcceptButtonClicked;
    }

    void OnAcceptButtonClicked()
    {
        gameInvite.style.display = DisplayStyle.None;
        miniGame.SetActive(true);
    }
    
    void OnDestroy()
    {
        // Unregister to prevent memory leaks
        if (gameAcceptButton != null)
        {
            gameAcceptButton.clicked -= OnAcceptButtonClicked;
        }
    }
}
