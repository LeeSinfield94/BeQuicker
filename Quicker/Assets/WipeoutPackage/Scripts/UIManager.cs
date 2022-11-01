using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _timerText;

    static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(_instance != this)
        {
            Destroy(_instance);
            _instance = this;
        }
        else
        {
            _instance = this;
        }
    }

    public void SetTimeText(float time)
    {
        _timerText.text = time.ToString();
    }
}
