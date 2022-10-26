using ExitGames.Client.Photon;
using MyGame.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float strafeSpeed;
    [SerializeField] PlayerAnimatorManager playerAnimator;
    [SerializeField] GameObject playerHUD;
    [SerializeField] float normalSpeed = 4;
    [SerializeField] ToggleHandler toggleHandler;

    float slowSpeed = 3f;
    bool isSlowed;
    public int currentLane = 1;
    int laneToPlaceTrap = 1;
    int otherPlayerCurrentLane = 1;

    public bool isReady;
    public Vector3 startPos;

    private bool canMoveForward = true;
    public bool CanMoveForward
    {
        set { canMoveForward = value; }
    }
    [SerializeField]
    PlayerFloor _myFloor;
    public PlayerFloor MyFloor
    {
        set { _myFloor = value; }
        get { return _myFloor; }
    }


    [SerializeField] float _speed;
    public float Speed
    {
        get { return _speed; }
        set 
        { 
            _speed = value;
            SetSpeed();
        }
    }

    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerController.LocalPlayerInstance = gameObject;
            PhotonNetwork.LocalPlayer.TagObject = gameObject;
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
        _speed = isSlowed ? slowSpeed : normalSpeed;
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

    public void CallUpdateTrapObjects()
    {
        photonView.RPC("UpdateTrapObjects", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void UpdateTrapObjects()
    {
        ObjectPooler.instance.GetAllSpawnedObjects();
    }

    public int GetOtherPlayersCurrentLane()
    {
        return otherPlayerCurrentLane;
    }

    public void SetCurrentLane(int lane)
    {
        laneToPlaceTrap = lane;
    }
    
    public void SetOtherPlayerCurrentLane(int otherCurrentLane)
    {
        otherPlayerCurrentLane = otherCurrentLane;
        if(toggleHandler != null)
        {
            toggleHandler.SetToggleColour(otherPlayerCurrentLane);
        }
    }

    private void FixedUpdate()
    {
        bool keyPressed = Input.GetKey(KeyCode.None);
        if (!keyPressed)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                LeftPressed();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                RightPressed();
            }
        }


        if (GameManager.AllPlayersReady)
        {
            SetSpeed();
            Movement(canMoveForward);
        }
        else
        {
            _speed = 0;
        }
        SetAnimationSpeed();
    }

    private void RightPressed()
    {
        currentLane++;
        if (currentLane >= _myFloor.Lanes.Count - 1)
        {
            currentLane = _myFloor.Lanes.Count - 1;
        }
    }

    private void LeftPressed()
    {
        currentLane--; 
        if (currentLane <= 0)
        {
            currentLane = 0;
        }
    }


    public void Movement(bool canMoveForward)
    {
        Vector3 movement = transform.position;

        if (canMoveForward)
        {
            movement += Vector3.forward * _speed * Time.deltaTime;
        }
        movement.x = _myFloor.Lanes[currentLane].position.x;
        transform.position = movement;
    }

    public void SetAnimationSpeed()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetAnimationSpeed(_speed);
        }
    }

    public void RaiseSpikeEvent()
    {
        if(TrapTimer.CanPlaceTrap)
        {
            object[] content = new object[] { laneToPlaceTrap };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(NetworkEventCodes.spawnSpikeEvent, content, raiseEventOptions, SendOptions.SendReliable);
            StartCoroutine(TrapTimer.WaitForTimer());
        }

    }

    public void SpawnSpike(ObstacleType type, int lane)
    {
        CallSpawnObstacleOnFloor(type, lane);
        photonView.RPC("SpawnSpikeForOthers", RpcTarget.Others, type, lane);
    }

    [PunRPC]
    public void SpawnSpikeForOthers(ObstacleType type, int lane)
    {
        CallSpawnObstacleOnFloor(type, lane);
    }

    private void CallSpawnObstacleOnFloor(ObstacleType type, int lane)
    {
        if (_myFloor)
        {
            _myFloor.SpawnObstacleOnFloor(type, lane, this);
        }
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
            isSlowed = (bool)stream.ReceiveNext();
            isReady = (bool)stream.ReceiveNext();
            currentLane = (int)stream.ReceiveNext();
            
            GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, isReady);

            if(_myFloor != null)
            {
                Movement(canMoveForward);
            }
            GameObject localPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            if (localPlayer != null)
            {
                localPlayer.GetComponent<PlayerController>().SetOtherPlayerCurrentLane(currentLane);
            }
            else
            {
                print("SHIIIIIT");
            }
        }
    }

}
