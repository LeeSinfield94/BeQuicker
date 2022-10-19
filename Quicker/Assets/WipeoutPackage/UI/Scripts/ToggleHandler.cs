using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles = new List<Toggle>();
    [SerializeField] private PlayerController player;

    private Toggle currentToggle;

    private void Update()
    {
        //if(player != null && player.OtherPlayer != null)
        //{

        //}
    }
    public void SetToggleColour(int otherPlayersCurrentLane)
    {
        if(currentToggle == toggles[otherPlayersCurrentLane])
        {
            return;
        }
        else
        {
            currentToggle = toggles[otherPlayersCurrentLane];
        }
    }

}
