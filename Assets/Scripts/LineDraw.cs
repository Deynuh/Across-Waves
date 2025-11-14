using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SimpleLineDraw : VisualElement
{
    List<List<Vector2>> allLines = new List<List<Vector2>>();
    List<Vector2> currentLine = new List<Vector2>();
    bool isDrawing = false;

    // NPC stuff
    List<Vector2> npcCompleteLines = new List<Vector2>();
    List<Vector2> npcCurrentLines = new List<Vector2>();
    bool npcIsDrawing = false;

    // bear drawing template guide!
    List<Vector2> templateLines = new List<Vector2>();

    public SimpleLineDraw()
    {
        RegisterCallback<PointerDownEvent>(evt =>
        {
            currentLine = new List<Vector2>();
            currentLine.Add(evt.localPosition);
            isDrawing = true;
            MarkDirtyRepaint();
        });

        RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (isDrawing)
            {
                currentLine.Add(evt.localPosition);
                MarkDirtyRepaint();
            }
        });

        RegisterCallback<PointerUpEvent>(evt =>
        {
            if (isDrawing)
            {
                allLines.Add(currentLine);
                isDrawing = false;
            }
        });

        generateVisualContent += OnGenerateVisualContent;
        pickingMode = PickingMode.Position;
    }

    public void SetTemplate(List<Vector2> template)
    {
        templateLines = template;
        MarkDirtyRepaint();
    }

    public void StartNPCDrawing (List<Vector2> linesToDraw, System.Action onComplete = null)
    {
        npcCompleteLines = linesToDraw;
        npcCurrentLines.Clear();
        npcIsDrawing = true;

        // start coroutine
        var coroutineRunner = GameObject.FindObjectOfType<DrawGameSetup>();
        coroutineRunner.StartCoroutine(AnimateNPCDrawing(onComplete));
    }

    public System.Collections.IEnumerator AnimateNPCDrawing(System.Action onComplete)
    {
        float drawSpeed = 80f;
        int currentPointIndex = 0;

        while (currentPointIndex < npcCompleteLines.Count)
        {
            //add next point
            npcCurrentLines.Add(npcCompleteLines[currentPointIndex]);
            currentPointIndex++;

            MarkDirtyRepaint();

            //wait based on distance to next point
            if (currentPointIndex < npcCompleteLines.Count)
            {
                float distance = Vector2.Distance(
                    npcCompleteLines[currentPointIndex - 1], 
                    npcCompleteLines[currentPointIndex]
                );
                yield return new WaitForSeconds(distance / drawSpeed);
            }
        }

        npcIsDrawing = false;
        onComplete?.Invoke();
    }

    // clears player's drawing only
    public void ClearDrawing()
    {
        Debug.Log("ClearDrawing() called!");
        Debug.Log($"Clearing {allLines.Count} lines");
        
        allLines.Clear();
        currentLine.Clear();
        isDrawing = false;
        MarkDirtyRepaint();
        
        Debug.Log("Drawing cleared and repaint marked!");
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var painter = mgc.painter2D;

        // Draw template (very light, transparent guide)
        if (templateLines.Count > 1)
        {
            painter.strokeColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Light gray, transparent
            painter.lineWidth = 5f;
            painter.BeginPath();
            painter.MoveTo(templateLines[0]);
            for (int i = 1; i < templateLines.Count; i++)
                painter.LineTo(templateLines[i]);
            painter.Stroke();
        }

        // Draw NPC's animated drawing (blue)
        if (npcCurrentLines.Count > 1)
        {
            painter.strokeColor = Color.blue;
            painter.lineWidth = 5f;
            painter.BeginPath();
            painter.MoveTo(npcCurrentLines[0]);
            for (int i = 1; i < npcCurrentLines.Count; i++)
                painter.LineTo(npcCurrentLines[i]);
            painter.Stroke();
        }

        // player's drawing lines
        painter.strokeColor = Color.red;
        painter.lineWidth = 5f;

         // draw all completed lines
        foreach (var line in allLines)
        {
            if (line.Count < 2) continue;
            
            painter.BeginPath();
            painter.MoveTo(line[0]);
            for (int i = 1; i < line.Count; i++)
                painter.LineTo(line[i]);
            painter.Stroke();
        }

        // Draw current line being drawn
        if (isDrawing && currentLine.Count >= 2)
        {
            painter.BeginPath();
            painter.MoveTo(currentLine[0]);
            for (int i = 1; i < currentLine.Count; i++)
                painter.LineTo(currentLine[i]);
            painter.Stroke();
        }
    }
}
