using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;


using System.Collections;
using TMPro;

namespace MyGame.UI
{
    /// <summary>
    /// Player name input field. Let the user input his name, will appear above the player in the game.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";

        void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = this.GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    inputField.text = defaultName;
                }
            }
            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;


            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

    }
}
