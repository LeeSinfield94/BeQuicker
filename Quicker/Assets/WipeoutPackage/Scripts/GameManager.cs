using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private static Dictionary<PlayerController, float> playerTime = new Dictionary<PlayerController, float>();
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private static bool allPlayersReady = false;
    public static bool AllPlayersReady
    {
        get { return allPlayersReady; }
        set { allPlayersReady = value; }
    }

    public delegate void RestartLevel();
    public event RestartLevel restartLevel;

    //list of players and their ready status.   
    public static Dictionary<object, bool> playersReady = new Dictionary<object, bool>();
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        allPlayersReady = false;
        if (PlayerController.LocalPlayerInstance == null)
        {
            int currentNumberOfPlayers = PhotonNetwork.PlayerList.Length - 1;
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject go = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[currentNumberOfPlayers].position, Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    public override void OnLeftRoom()
    {
        restartLevel.Invoke();
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        print("EnteredRoom");
    }


    public void AddPlayerToList(object playerTagObject)
    {
        if(!playersReady.ContainsKey(playerTagObject))
        {
            playersReady.Add(playerTagObject, false);
        }
    }
    public void RemovePlayerToList(object playerTagObject)
    {
        if (playersReady.ContainsKey(playerTagObject))
        {
            playersReady.Remove(playerTagObject);
        }
    }
    public static void SetPlayerTime(PlayerController player)
    {
        if(!playerTime.ContainsKey(player))
        {
            playerTime.Add(player, RaceTimer.GetCurrentTime());
        }
    }

    public static void SetPlayerReadyStatus(object playerTagObject, bool isReady)
    {
        if (playersReady.ContainsKey(playerTagObject))
        {
            playersReady[playerTagObject] = isReady;
        }
        
        CheckAllPlayersAreReady();
    }

   

    public static void CheckAllPlayersAreReady()
    {
        foreach(KeyValuePair<object, bool> player in playersReady)
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

    public static float GetPlayersCurrentTime(PlayerController player)
    {
        float time;
        playerTime.TryGetValue(player, out time);
        return time;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.UserId != null)
        {
            RemovePlayerToList(otherPlayer.UserId);
        }
        restartLevel.Invoke();
        allPlayersReady = false;
        print("Player Left Room");
    }

}