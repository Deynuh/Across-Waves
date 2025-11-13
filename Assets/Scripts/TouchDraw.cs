using System.Collections;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{
    private Coroutine drawing;
    [SerializeField] public GameObject linePrefab;
    private LineRenderer currentLineRenderer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        } 
        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }

    void StartLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }

        GameObject newLine = Instantiate(linePrefab);
        currentLineRenderer = newLine.GetComponent<LineRenderer>();

        drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }
    }

    IEnumerator DrawLine()
    {
        currentLineRenderer.positionCount = 0;

        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount-1, position);
            yield return null;
        }
    }
}
