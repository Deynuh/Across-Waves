using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ArtManager : MonoBehaviour
{
    [SerializeField] private Sprite[] eloSprites;
    [SerializeField] private Sprite[] umiSprites;

    private VisualElement callScreen;
    private VisualElement playerCam;

    private int currentSpriteIndex = 0;

    void Start()
    {
        callScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("CallScreen");
        playerCam = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("PlayerCam");
        SetBackgroundSprites(0);
    }

    public void SetBackgroundSprites(int spriteIndex)
    {
        currentSpriteIndex = spriteIndex;

        int eloIndex = Mathf.Clamp(spriteIndex, 0, eloSprites.Length - 1);
        int umiIndex = Mathf.Clamp(spriteIndex, 0, umiSprites.Length - 1);

        if (callScreen != null && eloIndex < eloSprites.Length && eloSprites[eloIndex] != null)
        {
            callScreen.style.backgroundImage = new StyleBackground(eloSprites[eloIndex]);
        }

        if (playerCam != null && umiIndex < umiSprites.Length && umiSprites[umiIndex] != null)
        {
            playerCam.style.backgroundImage = new StyleBackground(umiSprites[umiIndex]);
        }
    }

    public void NextSprites()
    {
        SetBackgroundSprites(currentSpriteIndex + 1);
    }

    public void PreviousSprites()
    {
        SetBackgroundSprites(currentSpriteIndex - 1);
    }

    public void ResetToFirst()
    {
        SetBackgroundSprites(0);
    }

    public void SelectSprites(int index)
    {
        SetBackgroundSprites(index);
    }
}
