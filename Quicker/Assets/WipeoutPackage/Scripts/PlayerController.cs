using ExitGames.Client.Photon;
using MyGame.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region SerializeFields

    [SerializeField] float _strafeSpeed;
    [SerializeField] PlayerAnimatorManager _playerAnimator;
    [SerializeField] GameObject _playerHUD;
    [SerializeField] float _normalSpeed = 4;
    [SerializeField] ToggleHandler _toggleHandler; 
    [SerializeField] PlayerHealthIconHandler _playerHealthIconHandler; 
    #endregion

    #region Private Variables
    float _slowSpeed = 3f;
    bool _isSlowed;
    public int _currentLane = 1;
    int _laneToPlaceTrap = 1;
    int _otherPlayerCurrentLane = 1;
    #endregion

    #region Public Variables

    public bool IsReady;
    public Vector3 StartPos;
    #endregion

    #region Properties
    [SerializeField]
    int _playerHealth = 5;
    public float PlayerHealth
    {
        get { return _playerHealth; }
    }
    bool _canMoveForward = true;
    public bool CanMoveForward
    {
        set { _canMoveForward = value; }
    }

    [SerializeField]
    PlayerFloor _myFloor;
    public PlayerFloor MyFloor
    {
        set { _myFloor = value; }
        get { return _myFloor; }
    } 
    #endregion


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
        GameManager.Instance.LevelRestarting += RestartPlayer;
        GameManager.Instance.OnPlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        GameManager.Instance.LevelRestarting -= RestartPlayer;
        GameManager.Instance.OnPlayerDied -= PlayerDied;
    }

    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CameraFollow.Instance.Init(this.transform);
            NetworkedEvents.Instance.LocalPlayer = this;
            StartPos = transform.position;
            RestartPlayer();
        }
        if(!photonView.IsMine)
        {
            _playerHUD.SetActive(false);
        }
    }

    #region Speed
    public void SetSlowed(bool isSlow)
    {
        _isSlowed = isSlow;
        SetSpeed();
    }

    public void SetSpeed()
    {
        _speed = _isSlowed ? _slowSpeed : _normalSpeed;
        SetAnimationSpeed();
    } 
    #endregion

    public void SetReadyUpStatus(bool readyStatus)
    {
        IsReady = readyStatus;
        GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, IsReady);
    }

    public void RestartPlayer()
    {
        transform.position = StartPos;
        IsReady = false;
        _playerHUD.GetComponentInChildren<TextHandler>().SetText("Not Ready");
        GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, this.IsReady);
    }

    public void CallUpdateTrapObjects()
    {
        photonView.RPC("UpdateTrapObjects", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void UpdateTrapObjects()
    {
        ObjectPooler.Instance.GetAllSpawnedObjects();
    }

    public int GetOtherPlayersCurrentLane()
    {
        return _otherPlayerCurrentLane;
    }

    public void SetCurrentLane(int lane)
    {
        _laneToPlaceTrap = lane;
    }
    
    public void SetOtherPlayerCurrentLane(int otherCurrentLane)
    {
        _otherPlayerCurrentLane = otherCurrentLane;
        if(_toggleHandler != null)
        {
            _toggleHandler.SetToggleColour(_otherPlayerCurrentLane);
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
            Movement(_canMoveForward);
        }
        else
        {
            _speed = 0;
        }
        SetAnimationSpeed();
    }

    #region Movement
    private void RightPressed()
    {
        _currentLane++;
        if (_currentLane >= _myFloor.Lanes.Count - 1)
        {
            _currentLane = _myFloor.Lanes.Count - 1;
        }
    }

    private void LeftPressed()
    {
        _currentLane--;
        if (_currentLane <= 0)
        {
            _currentLane = 0;
        }
    }


    public void Movement(bool canMoveForward)
    {
        Vector3 movement = transform.position;

        if (canMoveForward)
        {
            movement += Vector3.forward * _speed * Time.deltaTime;
        }
        movement.x = _myFloor.Lanes[_currentLane].position.x;
        transform.position = movement;
    }
    public void SetAnimationSpeed()
    {
        if (_playerAnimator != null)
        {
            _playerAnimator.SetAnimationSpeed(_speed);
        }
    }
    #endregion


    #region SpikeEvent
    public void RaiseSpikeEvent()
    {
        if (TrapTimer.CanPlaceTrap)
        {
            object[] content = new object[] { _laneToPlaceTrap };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(NetworkEventCodes.SpawnSpikeEvent, content, raiseEventOptions, SendOptions.SendReliable);
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
    #endregion

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

    public void ModifyHealth(bool positive, int amount)
    {
        _playerHealth = positive ? _playerHealth + amount : _playerHealth - amount;
        if(_playerHealthIconHandler != null)
        {
            _playerHealthIconHandler.UpdateHealthIcons(_playerHealth, Color.white);
        }
        if(IsDead())
        {
            PlayerDied();
        }
    }


    public bool IsDead()
    {
        return _playerHealth <= 0;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(_isSlowed);
            stream.SendNext(IsReady);
            stream.SendNext(_currentLane);
        }
        else
        {
            // Network player, receive data
            _isSlowed = (bool)stream.ReceiveNext();
            IsReady = (bool)stream.ReceiveNext();
            _currentLane = (int)stream.ReceiveNext();
            
            GameManager.SetPlayerReadyStatus(photonView.Controller.UserId, IsReady);

            if(_myFloor != null)
            {
                Movement(_canMoveForward);
            }
            GameObject localPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            if (localPlayer != null)
            {
                localPlayer.GetComponent<PlayerController>().SetOtherPlayerCurrentLane(_currentLane);
            }
        }

    }
    public void PlayerDied()
    {
        GameManager.Instance.LeaveRoom();
    }

}
