using UnityEngine;

public class WaveListener : MonoBehaviour
{
    [Header("References")]
    public WavePrompt wavePrompt; // Drag the GameObject that has the WavePrompt script attached

    [Header("Wave Detection Settings")]
    public float mouseMovementThreshold = 50f; // Minimum distance to register as a wave
    public float waveTimeWindow = 2f; // Time window to complete the wave gesture

    private bool isListening = false;
    private Vector3 lastMousePosition;
    private Vector3 startMousePosition;
    private float waveStartTime;
    private bool hasMovedRight = false;

    void Update()
    {
        // Check if wave prompt is visible and start listening
        if (wavePrompt != null && wavePrompt.IsWavePromptVisible() && !isListening)
        {
            StartListening();
        }

        // If listening, detect mouse movement
        if (isListening)
        {
            DetectWaveGesture();
        }
    }

    void StartListening()
    {
        isListening = true;
        lastMousePosition = Input.mousePosition;
        startMousePosition = Input.mousePosition;
        waveStartTime = Time.time;
        hasMovedRight = false;
        Debug.Log("Started listening for wave gesture...");
    }

    void DetectWaveGesture()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;

        // Check for right movement first
        if (!hasMovedRight && mouseDelta.x > 5f)
        {
            hasMovedRight = true;
            Debug.Log("Mouse moved right - continuing to listen for left movement...");
        }

        // Check for left movement after right movement
        if (hasMovedRight && mouseDelta.x < -5f)
        {
            float totalDistance = Mathf.Abs(currentMousePosition.x - startMousePosition.x);

            if (totalDistance >= mouseMovementThreshold)
            {
                Debug.Log("Wave gesture detected! User moved mouse left to right successfully!");
                StopListening();
                return;
            }
        }

        // Check if time window expired
        if (Time.time - waveStartTime > waveTimeWindow)
        {
            Debug.Log("Wave gesture timed out");
            StopListening();
        }

        lastMousePosition = currentMousePosition;
    }

    void StopListening()
    {
        isListening = false;
        hasMovedRight = false;
    }
}
