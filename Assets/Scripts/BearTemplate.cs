using UnityEngine;
using System.Collections.Generic;

public class BearTemplate {
    private Vector2 center;
    private float radius;

    public BearTemplate(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public List<List<Vector2>> CreateAllTemplates()
    {
        return new List<List<Vector2>>
        {
            CreateHeadTemplate(),
            CreateLeftEarTemplate(),
            CreateRightEarTemplate(),
            CreateLeftEyeTemplate(),
            CreateRightEyeTemplate(),
            CreateNoseTemplate(),
            CreateMouthTemplate()
        };
    }

    private List<Vector2> CreateHeadTemplate()
    {
        var head = new List<Vector2>();
        int headPointCount = 32;
        for (int i = 0; i <= headPointCount; i++)
        {
            float angle = i * 2f * Mathf.PI / headPointCount;
            Vector2 point = center + new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
            head.Add(point);
        }
        return head;
    }

    
    List<Vector2> CreateLeftEarTemplate()
    {
        var leftEar = new List<Vector2>();

        float earRadius = radius * 0.4f;
        Vector2 earCenter = center + new Vector2(-radius * 0.65f, -radius * 0.65f);

        for (int i = 0; i <= 16; i++)
        {
            // Start from bottom-left and curve toward top-right
            float angle = Mathf.PI * 0.75f + (i * Mathf.PI / 16);
            Vector2 point = earCenter + new Vector2(
                Mathf.Cos(angle) * earRadius,
                Mathf.Sin(angle) * earRadius
            );
            leftEar.Add(point);
        }

        return leftEar;
    }
    
    List<Vector2> CreateRightEarTemplate()
    {
        var rightEar = new List<Vector2>();

        float earRadius = radius * 0.4f;
        Vector2 earCenter = center + new Vector2(radius * 0.65f, -radius * 0.65f);

        for (int i = 0; i <= 16; i++)
        {
            // Start from bottom-right and curve toward top-left
            float angle = Mathf.PI * 0.25f - (i * Mathf.PI / 16);
            Vector2 point = earCenter + new Vector2(
                Mathf.Cos(angle) * earRadius,
                Mathf.Sin(angle) * earRadius
            );
            rightEar.Add(point);
        }

        return rightEar;
    }
    
    List<Vector2> CreateLeftEyeTemplate()
    {
        var leftEye = new List<Vector2>();

        float eyeRadius = radius * 0.08f; // Small eye size
        Vector2 eyeCenter = center + new Vector2(-radius * 0.3f, -radius * 0.2f);

        for (int i = 0; i <= 12; i++)
        {
            float angle = i * 2f * Mathf.PI / 12f;
            Vector2 point = eyeCenter + new Vector2(
                Mathf.Cos(angle) * eyeRadius,
                Mathf.Sin(angle) * eyeRadius
            );
            leftEye.Add(point);
        }

        return leftEye;
    }

    List<Vector2> CreateRightEyeTemplate()
    {
        var rightEye = new List<Vector2>();

        float eyeRadius = radius * 0.08f; // Small eye size
        Vector2 eyeCenter = center + new Vector2(radius * 0.3f, -radius * 0.2f);

        for (int i = 0; i <= 12; i++)
        {
            float angle = i * 2f * Mathf.PI / 12f;
            Vector2 point = eyeCenter + new Vector2(
                Mathf.Cos(angle) * eyeRadius,
                Mathf.Sin(angle) * eyeRadius
            );
            rightEye.Add(point);
        }

        return rightEye;
    }
    
    List<Vector2> CreateNoseTemplate()
    {
        var nose = new List<Vector2>();

        float noseRadius = radius * 0.04f; // Very small nose
        Vector2 noseCenter = center + new Vector2(0f, -radius * 0.05f);

        for (int i = 0; i <= 8; i++)
        {
            float angle = i * 2f * Mathf.PI / 8f;
            Vector2 point = noseCenter + new Vector2(
                Mathf.Cos(angle) * noseRadius,
                Mathf.Sin(angle) * noseRadius
            );
            nose.Add(point);
        }

        return nose;
    }

    List<Vector2> CreateMouthTemplate()
    {
        var mouth = new List<Vector2>();

        Vector2 mouthCenter = center + new Vector2(0f, radius * 0.15f);
        float mouthRadius = radius * 0.12f;

        // Create an upward-facing semicircle smile
        for (int i = 0; i <= 12; i++)
        {
            // smile
            float angle = Mathf.PI - (i * Mathf.PI / 12f);
            Vector2 point = mouthCenter + new Vector2(
                Mathf.Cos(angle) * mouthRadius,
                Mathf.Sin(angle) * mouthRadius
            );
            mouth.Add(point);
        }

        return mouth;
    }
}