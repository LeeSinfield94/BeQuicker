using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Launcher
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        string _gameVersion = "1";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] byte _maxPlayersPerRoom = 2;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] GameObject _controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField] GameObject _progressLabel;
        [SerializeField] GameObject _hostGameOptionsPanel;
        [SerializeField] GameObject _joinGameOptionsPanel;

        bool _isConnecting;
        bool _isHosting = false;
        bool _joiningRandomRoom = true;

        string _roomName = "";

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
        }

        public void HostGame()
        {
            _controlPanel.SetActive(false);
            _hostGameOptionsPanel.SetActive(true);
        }
        public void JoinGame()
        {
            _controlPanel.SetActive(false);
            _joinGameOptionsPanel.SetActive(true);
        }

        public void SetRoomName(string roomName)
        {
            _roomName = roomName;
        }

        public void StartHostGame()
        {
            _isHosting = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = _maxPlayersPerRoom, PublishUserId = true });
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }

        public void JoinGame(bool joiningRandomRoom)
        {
            _isHosting = false;
            _joiningRandomRoom = joiningRandomRoom;
            _progressLabel.SetActive(true);
            _controlPanel.SetActive(false);
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;

        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            if (_isConnecting)
            {
                if(_isHosting)
                {
                    PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = _maxPlayersPerRoom, PublishUserId = true });
                }
                else if(_joiningRandomRoom)
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {
                    PhotonNetwork.JoinRoom(_roomName);
                }
                _isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", message);
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayersPerRoom, PublishUserId = true });
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            print(message);
            _joinGameOptionsPanel.SetActive(false);
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("RaceScene");
        }
    }


}
