using MyGame.Gameplay;
using Photon.Pun;
using System.Collections;
using UnityEngine;
public class PlayerData : MonoBehaviourPun, IPunObservable
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
    public bool isReady;

    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerData.LocalPlayerInstance = this.gameObject;
        }
        GameManager.playersReady.Add(photonView.ViewID, isReady);
    }
    
    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CameraFollow.instance.Init(this.transform);
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
        GameManager.SetPlayerReadyStatus(photonView.ViewID, isReady);
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
        }

        if(!photonView.IsMine)
            GameManager.SetPlayerReadyStatus(photonView.ViewID, this.isReady);
    }
}
