using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private float startSpeed;
    [SerializeField] private PlayerFloor myFloor;
    [SerializeField] private PlayerFloor opponentsFloor;
    public PlayerFloor MyFloor
    {
        set 
        { 
            myFloor = value;
        }
    }
    public PlayerFloor OpponentsFloor
    {
        set
        {
            opponentsFloor = value;
        }
    }
    private float slowSpeed = 2;


    private bool canMoveForward = true;
    public bool CanMoveForward
    {
        set { canMoveForward = value; }
    }

    private Vector3 currentForward;

    private void Awake()
    {

    }

    public void SetSpeed(bool isSlow)
    {

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
            currentForward += Vector3.forward;
        }
        else
        {
            currentForward = new Vector3(0, 0, 0);
        }
    }

    public void SpawnSpikeForOtherPlayer()
    {
        opponentsFloor.SpawnObstacleOnFloor(ObstacleType.SPIKE);
    }

    public void SpawnSlowForOtherPlayer()
    {
        opponentsFloor.SpawnObstacleOnFloor(ObstacleType.SLOW);
    }

    public void SetMyTime()
    {
        GameManager.SetPlayerTime(this);
    }
}
