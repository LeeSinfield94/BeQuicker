using ExitGames.Client.Photon;
using MyGame.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private PlayerAnimatorManager playerAnimator;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private float normalSpeed = 4;

    private float slowSpeed = 3f;
    private bool isSlowed;
    private int currentLane = 1;
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
    public Vector3 startPos;
    

    public const byte spawnSpikeEvent = 1;
    public const byte spawnSlowEvent = 2;

    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerData.LocalPlayerInstance = this.gameObject;
        }

        GameManager.Instance.AddPlayerToList(photonView.Controller.UserId);
    }

    private void OnEnable()
    {
        GameManager.Instance.restartLevel += RestartPlayer;
    }

    private void OnDisable()
    {
        GameManager.Instance.restartLevel -= RestartPlayer;
    }

    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CameraFollow.instance.Init(this.transform);
            NetworkedEvents.instance.localPlayer = this;
            startPos = transform.position;
            RestartPlayer();
        }
        if(!photonView.IsMine)
        {
            playerHUD.SetActive(false);
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
        SetAnimationSpeed();
    }
    public void SetReadyUpStatus(bool readyStatus)
    {
        isReady = readyStatus;
        GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, isReady);
    }

    public void RestartPlayer()
    {
        transform.position = startPos;
        isReady = false;
        playerHUD.GetComponentInChildren<TextHandler>().SetText("Not Ready");
        GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, this.isReady);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane--;
            if (currentLane <= 0)
            {
                currentLane = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane++;
            if (currentLane >= myFloor.Lanes.Count - 1)
            {
                currentLane = myFloor.Lanes.Count - 1;
            }
        }
    }
    private void FixedUpdate()
    {
        //Player should not start moving until all other players are ready.
        if (GameManager.AllPlayersReady)
        {
            SetSpeed();
            Movement(canMoveForward);
        }
        else
        {
            speed = 0;
        }
        SetAnimationSpeed();
    }

    public void Movement(bool canMoveForward)
    {
        Vector3 movement = transform.position;

        if (canMoveForward)
        {
            movement += Vector3.forward * speed * Time.fixedDeltaTime;
        }
        movement.x = myFloor.Lanes[currentLane].position.x;
        print(currentLane);
        transform.position = movement;
    }

    public void SetAnimationSpeed()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetAnimationSpeed(speed);
        }
    }

    public void RaiseSlowEvent()
    {
        print("Test");
        object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(spawnSlowEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void RaiseSpikeEvent()
    {
        byte obstacleType = (byte)ObstacleType.SPIKE;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
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
            stream.SendNext(currentLane);
        }
        else
        {
            // Network player, receive data
            this.isSlowed = (bool)stream.ReceiveNext();
            this.isReady = (bool)stream.ReceiveNext();
            this.currentLane = (int)stream.ReceiveNext();
            print($"This ready up status = {isReady}");
            GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, this.isReady);

            if(myFloor != null)
            {
                Vector3 movement = transform.position;
                movement.x = myFloor.Lanes[currentLane].position.x;
                this.transform.position = movement;
            }


        }
    }

}
