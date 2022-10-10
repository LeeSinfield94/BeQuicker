using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedEvents : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkedEvents instance;
    public PlayerData localPlayer;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        switch(eventCode)
        {
            case 1:
                HandleSpikeEvent();
                break;
            case 2:
                HandleSlowEvent();
                break;
        }
    }

    public void HandleSpikeEvent()
    {
        if(localPlayer != null)
        {
            localPlayer.MyFloor.SpawnObstacleOnFloor(ObstacleType.SPIKE);
        }
    }

    public void HandleSlowEvent()
    {
        if (localPlayer != null)
        {
            localPlayer.MyFloor.SpawnObstacleOnFloor(ObstacleType.SLOW);
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


}
