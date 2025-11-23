using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WavePrompt : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _wavePromptElement;
    private bool _isComplete;
    
    public int totalWavesNeeded = 10; // change as needed for each call
    private int _waveCount;
    private bool _wasMovingRight;
    private bool _hasStartedMoving;
    
    private float _oscillateSpeed = 3f; 
    private float _oscillateRange = 75f; 
    private float _oscillateCounter;


    void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;
        _wavePromptElement = _root.Q("WavePrompt");
    
        // Don't show immediately - wait for DialogueController to activate
        if (_wavePromptElement != null)
        {
            _wavePromptElement.style.display = DisplayStyle.None;
        }
    }
    
    void OnEnable()
    {
        // Show when GameObject is activated by DialogueController
        _waveCount = 0;
        _isComplete = false;
        ShowWavePrompt();
    }
    
    void Update()
    {
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
    private void MoveWavePromptSideToSide()
    {
        if (_wavePromptElement != null && !_isComplete)
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

 
    private void WaveListener()
    {
        if (!_isComplete)
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
                        _isComplete = true;
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
    
    public bool IsWaveComplete()
    {
        return _isComplete;
    }
}