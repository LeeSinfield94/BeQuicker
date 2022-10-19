using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles = new List<Toggle>();
    [SerializeField] private PlayerController player;

    private UIColourSwtcher currentToggle;

    private void Update()
    {
        if (player != null)
        {
            SetToggleColour(player.GetOtherPlayersCurrentLane());
        }
    }
    public void SetToggleColour(int otherPlayersCurrentLane)
    {
        if(currentToggle != null && currentToggle.name == toggles[otherPlayersCurrentLane].GetComponent<UIColourSwtcher>().name)
        {
            currentToggle.backgroundImage.color = Color.red;
            return;
        }
        else
        {
            if(currentToggle != null)
                currentToggle.backgroundImage.color = currentToggle.normalColor;

            currentToggle = toggles[otherPlayersCurrentLane].GetComponent<UIColourSwtcher>();
            currentToggle.backgroundImage.color = Color.red;
        }
    }

}
