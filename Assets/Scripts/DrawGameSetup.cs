using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DrawGameSetup : MonoBehaviour
{
    private VisualElement canvas;
    private const float canvasWidth = 1350f;
    private const float canvasHeight = 800f;
    private Vector2 center = new Vector2(canvasWidth/2f, canvasHeight/2f);
    private float radius = canvasWidth/5f;
    void Start()
    {
        // Get Canvas
        canvas = GetComponent<UIDocument>().rootVisualElement.Q("Canvas");

        var titlePage = GetComponent<UIDocument>().rootVisualElement.Q("TitlePage");
        var clearButton = canvas.Q<Button>("ClearDrawing");

        // Create the drawing area first
        var drawArea = new LineDraw();
        drawArea.style.position = Position.Absolute;
        drawArea.style.top = 60;
        drawArea.style.left = 0;
        drawArea.style.right = 0;
        drawArea.style.bottom = 0;

        // Hide title page on first click
        bool firstClick = true;
        canvas.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (firstClick)
            {
                titlePage.style.display = DisplayStyle.None;
                clearButton.style.display = DisplayStyle.Flex;
                
                // set up bear template
                var bearTemplate = new BearTemplate(center, radius);
                var allTemplates = bearTemplate.CreateAllTemplates();
    
                foreach (var template in allTemplates)
                {
                    drawArea.AddTemplate(template);
                }

                // start npc drawing
                var npcDrawing = CreateNPCDrawing(allTemplates);
                drawArea.StartNPCDrawing(npcDrawing, () =>
                {
                    Debug.Log("NPC finished!");
                });
                
                firstClick = false;
            }        
        });

        // clear button listener
        clearButton.clicked += () => {
            Debug.Log("Clear button clicked!");
            drawArea.ClearDrawing();
        };

        // Add it to UI
        canvas.Add(drawArea);
    }
    
    void Update()
    {
        // Ctrl+Z undo functionality
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
        {
            var drawArea = canvas.Q<LineDraw>();
            if (drawArea != null)
            {
                drawArea.Undo();
            }
        }
    }
    
    List<Vector2> CreateNPCDrawing(List<List<Vector2>> allTemplates)
    {
        var npcLines = new List<Vector2>();

        for (int i = 0; i < allTemplates.Count; i++)
        {
            var template = allTemplates[i];

            if (i == 0) // Head - include left half only
            {
                var leftHalfPoints = new List<Vector2>();
                foreach (var point in template)
                {
                    if (point.x <= center.x + (radius * 0.1f))
                    {
                        leftHalfPoints.Add(point);
                    }
                }
                if (leftHalfPoints.Count > 0)
                {
                    npcLines.AddRange(leftHalfPoints);
                    npcLines.Add(new Vector2(float.NaN, float.NaN));
                }
            }
            else if (i == 1 || i == 3 || i == 5) // Left ear (1), Left eye (3), Nose (5) - include completely
            {
                npcLines.AddRange(template);
                npcLines.Add(new Vector2(float.NaN, float.NaN));
            }
            // Skip: Right ear (2), Right eye (4), Mouth (6)
        }

        // Remove the last line break if it exists
        if (npcLines.Count > 0 && float.IsNaN(npcLines[npcLines.Count - 1].x))
        {
            npcLines.RemoveAt(npcLines.Count - 1);
        }

        return npcLines;
    }

}

