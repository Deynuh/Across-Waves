using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DrawGameSetup : MonoBehaviour
{
    private VisualElement canvas;

    void Start()
    {
        // Get Canvas
        canvas = GetComponent<UIDocument>().rootVisualElement.Q("Canvas");

        var titlePage = GetComponent<UIDocument>().rootVisualElement.Q("TitlePage");
        var clearButton = canvas.Q<Button>("ClearDrawing");

        // Create the drawing area first
        var drawArea = new SimpleLineDraw();
        drawArea.style.position = Position.Absolute;
        drawArea.style.top = 30;
        drawArea.style.left = 0;
        drawArea.style.right = 0;
        drawArea.style.bottom = 0;

        // set up bear template
        var teddyBearTemplate = CreateTeddyBearTemplate();
        drawArea.SetTemplate(teddyBearTemplate);

        // Hide title page on first click
        bool firstClick = true;
        canvas.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (firstClick)
            {
                titlePage.style.display = DisplayStyle.None;
                clearButton.style.display = DisplayStyle.Flex;
                firstClick = false;
                // start npc drawing
                var npcHeadDrawing = CreateNPCHeadDrawing();
                drawArea.StartNPCDrawing(npcHeadDrawing, () =>
                {
                    Debug.Log("NPC finished!");
                });
            }        
        });

        // clear button listener
        clearButton.clicked += () => {
            Debug.Log("Clear button clicked!");
            drawArea.ClearDrawing();
        };

        // Add it to your UI
        canvas.Add(drawArea);
    }


    List<Vector2> CreateTeddyBearTemplate()
    {
        var template = new List<Vector2>();

        float canvasWidth = 1350f;
        float canvasHeight = 850f;

        Vector2 center = new Vector2(canvasWidth/2f, canvasHeight/2f);
        float radius = canvasWidth/5f; // Head radius
        
        // Head circle
        for (int i = 0; i <= 32; i++)
        {
            float angle = i * 2f * Mathf.PI / 32f;
            Vector2 point = center + new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
            template.Add(point);
        }
        
        // Left ear (scale with head)
        float earRadius = radius * 0.4f; // 40% of head size
        Vector2 leftEarCenter = center + new Vector2(-radius * 0.7f, -radius * 0.8f);
        for (int i = 0; i <= 16; i++)
        {
            float angle = i * 2f * Mathf.PI / 16f;
            Vector2 point = leftEarCenter + new Vector2(
                Mathf.Cos(angle) * earRadius,
                Mathf.Sin(angle) * earRadius
            );
            template.Add(point);
        }
        
        // Right ear
        Vector2 rightEarCenter = center + new Vector2(radius * 0.7f, -radius * 0.8f);
        for (int i = 0; i <= 16; i++)
        {
            float angle = i * 2f * Mathf.PI / 16f;
            Vector2 point = rightEarCenter + new Vector2(
                Mathf.Cos(angle) * earRadius,
                Mathf.Sin(angle) * earRadius
            );
            template.Add(point);
        }
        
        // Left eye (scale with head)
        float eyeSize = radius * 0.15f; // 15% of head radius
        Vector2 leftEyeCenter = center + new Vector2(-radius * 0.3f, -radius * 0.2f);
        for (int i = 0; i <= 8; i++)
        {
            float angle = i * 2f * Mathf.PI / 8f;
            Vector2 point = leftEyeCenter + new Vector2(
                Mathf.Cos(angle) * eyeSize,
                Mathf.Sin(angle) * eyeSize
            );
            template.Add(point);
        }
        
        // Right eye
        Vector2 rightEyeCenter = center + new Vector2(radius * 0.3f, -radius * 0.2f);
        for (int i = 0; i <= 8; i++)
        {
            float angle = i * 2f * Mathf.PI / 8f;
            Vector2 point = rightEyeCenter + new Vector2(
                Mathf.Cos(angle) * eyeSize,
                Mathf.Sin(angle) * eyeSize
            );
            template.Add(point);
        }
        
        // Nose (triangle, scaled with head)
        float noseSize = radius * 0.08f; // 8% of head radius
        Vector2 noseTop = center + new Vector2(0, radius * 0.1f);
        Vector2 noseLeft = center + new Vector2(-noseSize, radius * 0.25f);
        Vector2 noseRight = center + new Vector2(noseSize, radius * 0.25f);
        
        template.Add(noseTop);
        template.Add(noseLeft);
        template.Add(noseRight);
        template.Add(noseTop); // Close triangle
        
        // Mouth (simple smile curve)
        Vector2 mouthLeft = center + new Vector2(-radius * 0.15f, radius * 0.35f);
        Vector2 mouthCenter = center + new Vector2(0, radius * 0.45f);
        Vector2 mouthRight = center + new Vector2(radius * 0.15f, radius * 0.35f);
        
        template.Add(mouthLeft);
        template.Add(mouthCenter);
        template.Add(mouthRight);
        
        return template;
    }

    List<Vector2> CreateNPCHeadDrawing()
    {
        var npcLines = new List<Vector2>();
        
        // Use same centering as template
        float canvasWidth = 1350f;
        float canvasHeight = 850f;
        
        Vector2 center = new Vector2(canvasWidth / 2f, canvasHeight / 2f);
        float radius = canvasWidth/5f;
        
        // Only the head circle - NPC draws this part
        for (int i = 0; i <= 32; i++)
        {
            float angle = i * 2f * Mathf.PI / 32f;
            Vector2 point = center + new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
            npcLines.Add(point);
        }
        
        return npcLines;
    }

}

