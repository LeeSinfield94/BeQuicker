using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void SetText(string newText)
    {
        textMeshPro.text = newText;
    }
}
