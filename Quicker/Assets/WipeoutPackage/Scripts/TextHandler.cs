using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _textMeshPro.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void SetText(string newText)
    {
        _textMeshPro.text = newText;
    }
}
