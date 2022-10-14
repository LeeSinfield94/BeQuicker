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

    public void RestartTimer()
    {
        startTimer = false;
        timer = 0;
        UIManager.Instance.SetTimeText(timer);
    }
    public static float GetCurrentTime()
    {
        return timer;
    }
    private IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.restartLevel += RestartTimer;
    }
    private void OnEnable()
    {
        StartCoroutine(WaitForGameManager());
    }
    private void OnDisable()
    {
        GameManager.Instance.restartLevel -= RestartTimer;
    }
}
