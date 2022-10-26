using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    [SerializeField] private List<UIColourSwitcher> toggles = new List<UIColourSwitcher>();

    public void SetToggleColour(int otherPlayersCurrentLane)
    {
        for(int i = 0; i < toggles.Count; i++)
        {
            toggles[i].playerIsNotInLane = true;
            toggles[i].backgroundImage.color = toggles[i].normalColor;
        }
        toggles[otherPlayersCurrentLane].playerIsNotInLane = false;
        toggles[otherPlayersCurrentLane].backgroundImage.color = Color.red;
    }

}
