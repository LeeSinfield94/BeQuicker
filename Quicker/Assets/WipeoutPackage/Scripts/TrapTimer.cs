using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrapTimer
{
    private static bool canPlaceTrap = true;
    public static bool CanPlaceTrap
    {
        get { return canPlaceTrap; }
    }

    private static float timer = 3;

    
    public static IEnumerator WaitForTimer()
    {
        canPlaceTrap = false;
        yield return new WaitForSeconds(timer);
        canPlaceTrap = true;
    }
}
