using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrapTimer
{
    private static bool _canPlaceTrap = true;
    public static bool CanPlaceTrap
    {
        get { return _canPlaceTrap; }
    }

    private static float _timer = 3;

    
    public static IEnumerator WaitForTimer()
    {
        _canPlaceTrap = false;
        yield return new WaitForSeconds(_timer);
        _canPlaceTrap = true;
    }
}
