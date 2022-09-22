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
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public Transform spawnPoint;
    //list of players and their ready status.   
    public static Dictionary<PlayerData, bool> playersReady = new Dictionary<PlayerData, bool>();

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
        }
    }
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }


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