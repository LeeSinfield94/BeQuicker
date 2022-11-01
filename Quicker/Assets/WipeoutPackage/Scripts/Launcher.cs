using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Launcher
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        byte maxPlayersPerRoom = 2;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        GameObject progressLabel;
        [SerializeField]
        GameObject hostGameOptionsPanel;

        bool _isConnecting;
        bool _isHosting = false;

        string _roomName = "";

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public void HostGame()
        {
            controlPanel.SetActive(false);
            hostGameOptionsPanel.SetActive(true);
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
                PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true });
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void JoinGame()
        {
            _isHosting = false;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            if (_isConnecting)
            {
                if(_isHosting)
                {
                    PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true });
                }
                else
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                _isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", message);
            if(_roomName != "")
            {
                PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true });
            }
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true });
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("RaceScene");
        }
    }


}
