using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
  
    public void SetText(string newText)
    {
        textMeshPro.text = newText;
    }
}
