using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    public KeyCode testKey = KeyCode.Space;

    // Call this when the action finishes (e.g. animation event, coroutine end, trigger, etc.)
    public void OnWaveFinished()
    {
        GameEvents.RaiseActionComplete();
    }

    // Example: invoke after a timed action
    public void StartTimedAction(float seconds)
    {
        StartCoroutine(TimedAction(seconds));
    }

    private System.Collections.IEnumerator TimedAction(float s)
    {
        yield return new WaitForSeconds(s);
        GameEvents.RaiseActionComplete();
    }

    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            GameEvents.RaiseActionComplete();
            Debug.Log("GameEvents.RaiseActionComplete() called (test key)");
        }
    }

    [ContextMenu("Raise Action Complete")]
    private void RaiseFromContextMenu()
    {
        GameEvents.RaiseActionComplete();
        Debug.Log("Raised via ContextMenu");
    }
}