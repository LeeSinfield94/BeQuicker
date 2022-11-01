using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    private static float _timer = 0f;
    private static bool _startTimer = false;
    public static bool StartTimer
    {
        set { _startTimer = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (_startTimer)
        {
            _timer += Time.deltaTime;
            UIManager.Instance.SetTimeText(_timer); 
        }
    }

    public void RestartTimer()
    {
        _startTimer = false;
        _timer = 0;
        UIManager.Instance.SetTimeText(_timer);
    }
    public static float GetCurrentTime()
    {
        return _timer;
    }
    private IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.LevelRestarting += RestartTimer;
    }
    private void OnEnable()
    {
        StartCoroutine(WaitForGameManager());
    }
    private void OnDisable()
    {
        GameManager.Instance.LevelRestarting -= RestartTimer;
    }
}
