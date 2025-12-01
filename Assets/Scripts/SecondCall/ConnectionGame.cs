using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionGame : MonoBehaviour
{
    private ProgressBar[] signalBars;
    [SerializeField] private GameObject endDialogue;

    private int currentBarIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        signalBars = new ProgressBar[5];
        for (int i = 0; i < 5; i++)
        {
            signalBars[i] = root.Q<ProgressBar>($"Signal{i + 1}");

            int barIndex = i; // need this for OnBarClicked closure
            signalBars[i].RegisterCallback<ClickEvent>(evt => OnBarClicked(barIndex));
        }
    }

    void OnBarClicked(int clickedBarIndex)
    {
        Debug.Log($"Bar {clickedBarIndex + 1} clicked!");

        // check if player on correct bar
        if (clickedBarIndex == currentBarIndex)
        {
            // fills bar
            signalBars[currentBarIndex].value = 100.0f;
            currentBarIndex++;

            // check if all bars filled
            if (currentBarIndex >= signalBars.Length)
            {
                Debug.Log("All bars filled.");
                endDialogue.SetActive(true);
                gameObject.SetActive(false);
                // GetComponent<UIDocument>().enabled = false;
            }
        }
        // wrong bar clicked
        else
        {
            Debug.Log("Wrong bar clicked. Resetting progress.");
            currentBarIndex = 0;
            foreach (var bar in signalBars)
            {
                bar.value = 0.0f;
            }
        }
    }
}
