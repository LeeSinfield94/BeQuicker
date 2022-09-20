using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    private static float timer = 0f;
    private static bool startTimer = false;
    public static bool StartTimer
    {
        set { startTimer = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            UIManager.Instance.SetTimeText(timer); 
        }
    }

    public static float GetCurrentTime()
    {
        return timer;
    }
}
