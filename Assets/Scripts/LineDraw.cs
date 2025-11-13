using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SimpleLineDraw : VisualElement
{
    List<Vector2> points = new List<Vector2>();
    bool isDrawing = false;

    public SimpleLineDraw()
    {
        RegisterCallback<PointerDownEvent>(evt =>
        {
            points.Clear();
            points.Add(evt.localPosition);
            isDrawing = true;
            MarkDirtyRepaint();
        });

        RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (isDrawing)
            {
                points.Add(evt.localPosition);
                MarkDirtyRepaint();
            }
        });

        RegisterCallback<PointerUpEvent>(evt =>
        {
            isDrawing = false;
        });

        generateVisualContent += OnGenerateVisualContent;
        pickingMode = PickingMode.Position;
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        if (points.Count < 2) return;

        var painter = mgc.painter2D;
        painter.strokeColor = Color.black;
        painter.lineWidth = 2f;
        painter.BeginPath();
        painter.MoveTo(points[0]);

        for (int i = 1; i < points.Count; i++)
            painter.LineTo(points[i]);

        painter.Stroke();
    }
}
