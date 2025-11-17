using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class LineDraw : VisualElement
{
    List<List<Vector2>> allLines = new List<List<Vector2>>();
    List<Vector2> currentLine = new List<Vector2>();
    bool isDrawing = false;

    // NPC stuff
    List<Vector2> npcCompleteLines = new List<Vector2>();
    List<Vector2> npcCurrentLines = new List<Vector2>();

    // bear drawing template guide!
    List<List<Vector2>> allTemplateLines = new List<List<Vector2>>();

    public LineDraw()
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

    public void AddTemplate(List<Vector2> template)
    {
        allTemplateLines.Add(template);
        MarkDirtyRepaint();
    }
    
    public void StartNPCDrawing (List<Vector2> linesToDraw, System.Action onComplete = null)
    {
        npcCompleteLines = linesToDraw;
        npcCurrentLines.Clear();

        // start coroutine
        var coroutineRunner = GameObject.FindFirstObjectByType<DrawGameSetup>();
        coroutineRunner.StartCoroutine(AnimateNPCDrawing(onComplete));
    }

    public System.Collections.IEnumerator AnimateNPCDrawing(System.Action onComplete)
    {
        float drawSpeed = 80f;
        int currentPointIndex = 0;

        while (currentPointIndex < npcCompleteLines.Count)
        {
            Vector2 currentPoint = npcCompleteLines[currentPointIndex];

            // Check if this is a line break marker
            if (float.IsNaN(currentPoint.x))
            {
                // Add the line break marker to the current drawing
                npcCurrentLines.Add(currentPoint);
                currentPointIndex++;
                MarkDirtyRepaint();
                continue;
            }

            // Add the actual drawing point
            npcCurrentLines.Add(currentPoint);
            currentPointIndex++;

            MarkDirtyRepaint();

            // Wait based on distance to next point (only if next point exists and isn't a line break)
            if (currentPointIndex < npcCompleteLines.Count)
            {
                Vector2 nextPoint = npcCompleteLines[currentPointIndex];

                // Only calculate wait time if next point is not a line break
                if (!float.IsNaN(nextPoint.x))
                {
                    float distance = Vector2.Distance(currentPoint, nextPoint);
                    yield return new WaitForSeconds(distance / drawSpeed);
                }
            }
        }

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
    
    public void Undo()
    {
        if (allLines.Count > 0)
        {
            allLines.RemoveAt(allLines.Count - 1);
            MarkDirtyRepaint(); // Refresh the visual
        }
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var painter = mgc.painter2D;
        
        // Draw all template parts (very light, transparent guide)
        foreach (var templateLine in allTemplateLines)
        {
            if (templateLine.Count > 1)
            {
                painter.strokeColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                painter.lineWidth = 5f;
                painter.BeginPath();
                painter.MoveTo(templateLine[0]);
                for (int i = 1; i < templateLine.Count; i++)
                    painter.LineTo(templateLine[i]);
                painter.Stroke();
            }
        }

        // Draw NPC's animated drawing (blue) - handle line breaks properly
        if (npcCurrentLines.Count > 0)
        {
            painter.strokeColor = Color.blue;
            painter.lineWidth = 5f;
            
            var currentSegment = new List<Vector2>();
            
            foreach (var point in npcCurrentLines)
            {
                if (float.IsNaN(point.x)) // Line break marker
                {
                    // Draw current segment if it has points
                    if (currentSegment.Count > 1)
                    {
                        painter.BeginPath();
                        painter.MoveTo(currentSegment[0]);
                        for (int i = 1; i < currentSegment.Count; i++)
                            painter.LineTo(currentSegment[i]);
                        painter.Stroke();
                    }
                    currentSegment.Clear(); // Start new segment
                }
                else
                {
                    currentSegment.Add(point); // Add to current segment
                }
            }
            
            // Draw the final segment
            if (currentSegment.Count > 1)
            {
                painter.BeginPath();
                painter.MoveTo(currentSegment[0]);
                for (int i = 1; i < currentSegment.Count; i++)
                    painter.LineTo(currentSegment[i]);
                painter.Stroke();
            }
        }

        // player's drawing lines (unchanged)
        painter.strokeColor = Color.red;
        painter.lineWidth = 5f;

        foreach (var line in allLines)
        {
            if (line.Count < 2) continue;
            
            painter.BeginPath();
            painter.MoveTo(line[0]);
            for (int i = 1; i < line.Count; i++)
                painter.LineTo(line[i]);
            painter.Stroke();
        }

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
