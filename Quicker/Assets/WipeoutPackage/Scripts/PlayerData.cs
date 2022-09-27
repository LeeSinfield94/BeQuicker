using MyGame.Gameplay;
using Photon.Pun;
using System.Collections;
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
        StartCoroutine(WaitForOneSecond());
    }
    public IEnumerator WaitForOneSecond()
    {
        yield return new WaitForSeconds(2);
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Player Pos = {gameObject.transform.position}");
                gameObject.transform.position = GameManager.instance.spawnPoints[0].transform.position;
                Debug.Log($"Spawn Point 0 Pos = {gameObject.transform.position}");
            }
            else
            {
                transform.position = GameManager.instance.spawnPoints[1].transform.position;
                Debug.Log($"Spawn Point 1 Pos = {GameManager.instance.spawnPoints[1].transform.position}");
            }
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

        Debug.Log($"Spawn Point 0 Pos = {gameObject.transform.position}");
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
        }
        else
        {
            // Network player, receive data
            this.isSlowed = (bool)stream.ReceiveNext();
        }
    }
}
