using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SimpleLineDraw : VisualElement
{
    List<List<Vector2>> allLines = new List<List<Vector2>>();
    List<Vector2> currentLine = new List<Vector2>();
    bool isDrawing = false;

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

    public void ClearDrawing()
    {
        allLines.Clear();
        currentLine.Clear();
        isDrawing = false;
        MarkDirtyRepaint();
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var painter = mgc.painter2D;
        painter.strokeColor = Color.red;
        painter.lineWidth = 5f;

         // Draw all completed lines
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
