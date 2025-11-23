using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WavePrompt : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _wavePromptElement;

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;

        // Find your UI element by name/class
        _wavePromptElement = _root.Q("WavePrompt");

        // Initially hide it
        _wavePromptElement.style.display = DisplayStyle.None;
    }

    void Update()
    {
        // Currently using B to make wave prompt show up for testing
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowWavePrompt();
        }
        
        MoveWavePromptSideToSide();
        WaveListener();
    }

    private void ShowWavePrompt()
    {
        if (_wavePromptElement != null)
        {
            _wavePromptElement.style.display = DisplayStyle.Flex;
            _wavePromptElement.style.opacity = 0f; // Start fully transparent
            
            // Add transition for smooth fade
            _wavePromptElement.style.transitionDuration = new List<TimeValue> { new TimeValue(0.5f) };
            _wavePromptElement.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("opacity") };
        
            // Fade in after a small delay to ensure display is set
            StartCoroutine(FadeIn());
            
            Debug.Log("WavePrompt element shown!");
        }
    }
    
    private float _oscillateSpeed = 3f; 
    private float _oscillateRange = 75f; 
    private float _oscillateCounter;

    private void MoveWavePromptSideToSide()
    {
        if (_wavePromptElement != null && IsWavePromptVisible())
        {
            // Use Time.deltaTime for frame rate independent movement
            _oscillateCounter += _oscillateSpeed * Time.deltaTime;

            // Calculate oscillation using sine wave
            float oscillation = Mathf.Sin(_oscillateCounter) * _oscillateRange;

            // Center position minus half the element's width + oscillation
            float centerX = (Screen.width * 0.5f) - (_wavePromptElement.resolvedStyle.width * 0.5f);
            float newLeft = centerX + oscillation;

            _wavePromptElement.style.left = newLeft;
        }
    }


    private void HideWavePrompt()
    {
        if (_wavePromptElement != null)
        {
            StartCoroutine(FadeOut());
            Debug.Log("WavePrompt element hidden!");
        }
    }
    
    private System.Collections.IEnumerator FadeIn()
    {
        yield return null; // Wait one frame
        _wavePromptElement.style.opacity = 1f; // Fade to visible
    }

    private System.Collections.IEnumerator FadeOut()
    {
        _wavePromptElement.style.opacity = 0f; // Fade to invisible
        yield return new WaitForSeconds(0.5f); // Wait for fade to complete
        _wavePromptElement.style.display = DisplayStyle.None; // Then hide
    }

    public int totalWavesNeeded = 10; // change as needed for each call
    private int _waveCount;
    private bool _wasMovingRight;
    private bool _hasStartedMoving;
    private void WaveListener()
    {
        
        
        if (IsWavePromptVisible())
        {
            float mouseX = Input.GetAxis("Mouse X");
            if (Mathf.Abs(mouseX) > 0.1f)
            {
                Debug.Log("Player is moving mouse horizontally.");
                
                bool movingRight = mouseX > 0;
                
                // If we've started moving and direction changed
                if (_hasStartedMoving && _wasMovingRight != movingRight)
                {
                    _waveCount++;
                
                    if (_waveCount >= totalWavesNeeded)
                    {
                        Debug.Log(totalWavesNeeded + " waves complete!");
                        HideWavePrompt();
                        _waveCount = 0;
                        _hasStartedMoving = false;
                        return;
                    }
                }
            
                _wasMovingRight = movingRight;
                _hasStartedMoving = true;
            }

        }
    }
    
    private bool IsWavePromptVisible()
    {
        if (_wavePromptElement != null)
        {
            return _wavePromptElement.style.display == DisplayStyle.Flex && 
                   _wavePromptElement.style.opacity.value > 0f;
        }
        return false;
    }
    
}