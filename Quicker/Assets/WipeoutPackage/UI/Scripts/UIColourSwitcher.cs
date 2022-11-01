using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIColourSwitcher : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Toggle toggle;
    public Image backgroundImage;
    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color selectedColor;
    public Color disabledColor;
    public bool isOn;
    public bool mouseOver = false;
    public bool playerIsNotInLane = true;

    public void Update()
    {
        isOn = toggle.isOn;
        if(!mouseOver && !isOn && playerIsNotInLane)
        {
            backgroundImage.color = normalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(backgroundImage != null)
        {
            backgroundImage.color = pressedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if(!isOn && backgroundImage != null)
        {
            backgroundImage.color = highlightedColor;
        }
        else
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        if (!isOn && backgroundImage != null)
        {
            backgroundImage.color = normalColor;
        }
        else
        {
            return;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isOn && backgroundImage != null)
        {
            backgroundImage.color = normalColor;
        }
        else
        {
            return;
        }
    }
}
