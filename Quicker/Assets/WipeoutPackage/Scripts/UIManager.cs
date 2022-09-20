using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(instance != this)
        {
            Destroy(instance);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public void SetTimeText(float time)
    {
        timerText.text = time.ToString();
    }
}
