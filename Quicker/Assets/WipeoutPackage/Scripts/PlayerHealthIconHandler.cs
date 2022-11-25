using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthIconHandler : MonoBehaviour
{
    [SerializeField] List<Image> _playerHealthImages;

    public void UpdateHealthIcons(int imageIndex, Color newColor)
    {
        print(imageIndex);
        _playerHealthImages[imageIndex].color = newColor;
    }
}
