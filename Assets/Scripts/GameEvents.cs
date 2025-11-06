// csharp
using System;

public static class GameEvents
{
    public static event Action OnActionComplete;

    public static void RaiseActionComplete()
    {
        OnActionComplete?.Invoke();
    }
}