using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIColourSwitcher : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Toggle Toggle;
    public Image BackgroundImage;
    public Color NormalColor;
    public Color HighlightedColor;
    public Color PressedColor;
    public Color SelectedColor;
    public Color DisabledColor;
    public bool IsOn;
    public bool MouseOver = false;
    public bool PlayerIsNotInLane = true;

    public void Update()
    {
        IsOn = Toggle.isOn;
        if(!MouseOver && !IsOn && PlayerIsNotInLane)
        {
            BackgroundImage.color = NormalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(BackgroundImage != null)
        {
            BackgroundImage.color = PressedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOver = true;
        if(!IsOn && BackgroundImage != null)
        {
            BackgroundImage.color = HighlightedColor;
        }
        else
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOver = false;
        if (!IsOn && BackgroundImage != null)
        {
            BackgroundImage.color = NormalColor;
        }
        else
        {
            return;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!IsOn && BackgroundImage != null)
        {
            BackgroundImage.color = NormalColor;
        }
        else
        {
            return;
        }
    }
}
