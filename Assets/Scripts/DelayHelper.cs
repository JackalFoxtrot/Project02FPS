using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class DelayHelper
{ 
    public static Coroutine DelayAction(this MonoBehaviour monobehaviour, Action action, float delayDuration)
    {
        return monobehaviour.StartCoroutine(DelayActionRoutine(action, delayDuration));
    }
    private static IEnumerator DelayActionRoutine(Action action, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        action();
    }
}
