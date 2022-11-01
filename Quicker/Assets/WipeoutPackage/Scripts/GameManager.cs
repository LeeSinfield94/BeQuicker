using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] static Dictionary<PlayerController, float> _playerTime = new Dictionary<PlayerController, float>();
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();

    private static bool _allPlayersReady = false;
    public static bool AllPlayersReady
    {
        get { return _allPlayersReady; }
    }

    public delegate void RestartLevel();
    public event RestartLevel LevelRestarting;

    //list of players and their ready status.   
    public static Dictionary<object, bool> playersReady = new Dictionary<object, bool>();
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        _allPlayersReady = false;
        if (PlayerController.LocalPlayerInstance == null)
        {
            int currentNumberOfPlayers = PhotonNetwork.PlayerList.Length - 1;
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject go = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[currentNumberOfPlayers].position, Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    public override void OnLeftRoom()
    {
        LevelRestarting.Invoke();
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
        if(!_playerTime.ContainsKey(player))
        {
            _playerTime.Add(player, RaceTimer.GetCurrentTime());
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
               _allPlayersReady = false;
               return;
            }
        }
        _allPlayersReady = true;
        StartTimer();
    }

    public static void StartTimer()
    {
        RaceTimer.StartTimer = true;
    }

    public static float GetPlayersCurrentTime(PlayerController player)
    {
        float time;
        _playerTime.TryGetValue(player, out time);
        return time;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.UserId != null)
        {
            RemovePlayerToList(otherPlayer.UserId);
        }
        LevelRestarting.Invoke();
        _allPlayersReady = false;
        print("Player Left Room");
    }

}