using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private static Dictionary<PlayerData, float> playerTime = new Dictionary<PlayerData, float>();
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private static byte SetPlayerIsReady = 1;
    private static bool allPlayersReady = false;
    public static bool AllPlayersReady
    {
        get { return allPlayersReady; }
        set { allPlayersReady = value; }
    }

    //list of players and their ready status.   
    public static Dictionary<PlayerData, bool> playersReady = new Dictionary<PlayerData, bool>();
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        if (PlayerData.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
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


    public override void OnPlayerEnteredRoom(Player other)
    {

    }

    public override void OnPlayerLeftRoom(Player other)
    {

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
        if (playersReady.ContainsKey(player))
        {
            playersReady[player] = isReady;
        }
        
        CheckAllPlayersAreReady();
    }

    //private static void UpdatePlayersReady()
    //{
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    //    PhotonNetwork.RaiseEvent(SetPlayerIsReady, GameManager.AllPlayersReady, raiseEventOptions, SendOptions.SendReliable);
    //}
   

    public static void CheckAllPlayersAreReady()
    {
        foreach(KeyValuePair<PlayerData, bool> player in playersReady)
        {
            if(player.Value == false)
            {
               allPlayersReady = false;
               //UpdatePlayersReady();
               return;
            }
        }
        allPlayersReady = true;
        //UpdatePlayersReady();
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
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    //public void OnEvent(EventData photonEvent)
    //{
    //    allPlayersReady = (bool)photonEvent.CustomData;
    //    Debug.Log($"All Players Ready = {allPlayersReady}");
    //    if (allPlayersReady)
    //    {
    //        StartTimer();
    //    }
    //}
}