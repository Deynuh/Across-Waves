using UnityEngine;
using UnityEngine.UIElements;

public class DrawGameSetup : MonoBehaviour
{
    void Start()
    {
        // Get your UI Document's root
        var root = GetComponent<UIDocument>().rootVisualElement.Q("Canvas");
        
        // Create the drawing area
        var drawArea = new SimpleLineDraw();
        drawArea.style.position = Position.Absolute;
        drawArea.style.top = 0;
        drawArea.style.left = 0;
        drawArea.style.right = 0;
        drawArea.style.bottom = 0;
        
        // Add it to your UI
        root.Add(drawArea);
    }

}

