using ExitGames.Client.Photon;
using MyGame.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private PlayerAnimatorManager playerAnimator;
    [SerializeField] private GameObject playerHUd;

    private float slowSpeed = 0.1f;
    private float normalSpeed = 1;
    private bool isSlowed;
    private bool canMoveForward = true;
    public bool CanMoveForward
    {
        set { canMoveForward = value; }
    }
    [SerializeField]
    private PlayerFloor myFloor;
    public PlayerFloor MyFloor
    {
        set { myFloor = value; }
        get { return myFloor; }
    }
    public bool isReady;

    public const byte spawnSpikeEvent = 1;
    public const byte spawnSlowEvent = 2;

    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerData.LocalPlayerInstance = this.gameObject;
        }

        GameManager.instance.AddPlayerToList(photonView.Controller.UserId);
    }
    
    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CameraFollow.instance.Init(this.transform);
            NetworkedEvents.instance.localPlayer = this;
        }
        if(!photonView.IsMine)
        {
            playerHUd.SetActive(false);
        }
    }

    public void SetSlowed(bool isSlow)
    {
        isSlowed = isSlow;
        SetSpeed();
    }

    public void SetSpeed()
    {
        speed = isSlowed ? slowSpeed : normalSpeed;
        SetAnimaionSpeed();
    }
    public void SetReadyUpStatus(bool readyStatus)
    {
        isReady = readyStatus;
        GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, isReady);
    }
    
    private void Update()
    {

        //Player should not start moving until all other players are ready.
        if (GameManager.AllPlayersReady)
        {
            SetSpeed();
        }
        else
        {
            speed = 0;
        }
        SetAnimaionSpeed();
    }

    public void SetAnimaionSpeed()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetAnimationSpeed(speed);
        }
    }

    public void RaiseSlowEvent()
    {
        byte obstacleType = (byte)ObstacleType.SLOW;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(spawnSlowEvent, obstacleType, raiseEventOptions, SendOptions.SendReliable);
    }

    public void RaiseSpikeEvent()
    {
        byte obstacleType = (byte)ObstacleType.SPIKE;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(spawnSpikeEvent, obstacleType, raiseEventOptions, SendOptions.SendReliable);
    }
    public void SetMyTime()
    {
        GameManager.SetPlayerTime(this);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isSlowed);
            stream.SendNext(isReady);
        }
        else
        {
            // Network player, receive data
            this.isSlowed = (bool)stream.ReceiveNext();
            this.isReady = (bool)stream.ReceiveNext();
            print($"This ready up status = {isReady}");
            GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, this.isReady);
        }

    }

}
