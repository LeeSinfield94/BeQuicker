using MyGame.Gameplay;
using Photon.Pun;
using UnityEngine;
public class PlayerData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private PlayerAnimatorManager playerAnimator;
    private float slowSpeed = 0.1f;
    private float normalSpeed = 1;
    private bool isSlowed;
    private bool canMoveForward = true;
    public bool CanMoveForward
    {
        set { canMoveForward = value; }
    }

    private Vector3 currentForward;

    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerData.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CameraFollow.instance.Init(this.transform);
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
        GameManager.SetPlayerReadyStatus(this, readyStatus);
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
        print(isSlowed);
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
        }
        else
        {
            // Network player, receive data
            this.isSlowed = (bool)stream.ReceiveNext();
        }
    }
}
