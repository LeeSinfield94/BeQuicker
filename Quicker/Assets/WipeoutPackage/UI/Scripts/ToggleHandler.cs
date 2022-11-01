using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    [SerializeField] List<UIColourSwitcher> _toggles = new List<UIColourSwitcher>();

    public void SetToggleColour(int otherPlayersCurrentLane)
    {
        for(int i = 0; i < _toggles.Count; i++)
        {
            _toggles[i].PlayerIsNotInLane = true;
            _toggles[i].BackgroundImage.color = _toggles[i].NormalColor;
        }
        _toggles[otherPlayersCurrentLane].PlayerIsNotInLane = false;
        _toggles[otherPlayersCurrentLane].BackgroundImage.color = Color.red;
    }

}
