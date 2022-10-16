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
        print(photonEvent.CustomData);
        switch (eventCode)
        {
            case 1:
                HandleSpikeEvent(photonEvent.CustomData);
                break;
        }
    }

    public void HandleSpikeEvent(object data)
    {
        print(data);
        int actorNumber = (int)data;
        print(actorNumber + "" + PhotonNetwork.LocalPlayer.ActorNumber);
        if(actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            localPlayer.MyFloor.SpawnObstacleOnFloor(ObstacleType.SPIKE);
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
