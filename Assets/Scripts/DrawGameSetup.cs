using UnityEngine;
using UnityEngine.UIElements;

public class DrawGameSetup : MonoBehaviour
{
    void Start()
    {
        // Get Canvas
        var canvas = GetComponent<UIDocument>().rootVisualElement.Q("Canvas");

        var titlePage = GetComponent<UIDocument>().rootVisualElement.Q("TitlePage");
        var clearButton = canvas.Q<Button>("ClearDrawing");

        // Hide title page on first click
        bool firstClick = true;
        canvas.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (firstClick)
            {
                titlePage.style.display = DisplayStyle.None;
                clearButton.style.display = DisplayStyle.Flex;
                firstClick = false;
            }
        });

        // Create the drawing area
        var drawArea = new SimpleLineDraw();
        drawArea.style.position = Position.Absolute;
        drawArea.style.top = 0;
        drawArea.style.left = 0;
        drawArea.style.right = 0;
        drawArea.style.bottom = 0;

        // Create clear button
        clearButton.clicked += () => drawArea.ClearDrawing();
        
        // Add it to your UI
        canvas.Add(drawArea);
        canvas.Add(clearButton);
    }

}

