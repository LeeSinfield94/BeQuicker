using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private static Dictionary<PlayerData, float> playerTime = new Dictionary<PlayerData, float>();
    private static bool allPlayersReady = false;
    public static bool AllPlayersReady
    {
        get { return allPlayersReady; }
    }

    //list of players and their ready status.   
    public static Dictionary<PlayerData, bool> playersReady = new Dictionary<PlayerData, bool>();
    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }


    #endregion
    #region Photon Callbacks


    public override void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        }
    }
    #endregion

    public static void SetPlayerTime(PlayerData player)
    {
        if(!playerTime.ContainsKey(player))
        {
            playerTime.Add(player, RaceTimer.GetCurrentTime());
        }
    }

    public static void SetPlayerReadyStatus(PlayerData player, bool isReady)
    {
        if(playersReady.ContainsKey(player))
        {
            playersReady[player] = isReady;
        }
        CheckAllPlayersAreReady();
    }

    public static void CheckAllPlayersAreReady()
    {
        foreach(KeyValuePair<PlayerData, bool> player in playersReady)
        {
            if(player.Value == false)
            {
               allPlayersReady = false;
               return;
            }
        }
        allPlayersReady = true;
        StartTimer();
    }

    public static void StartTimer()
    {
        RaceTimer.StartTimer = true;
    }

    public static float GetPlayersCurrentTime(PlayerData player)
    {
        float time;
        playerTime.TryGetValue(player, out time);
        return time;
    }
}