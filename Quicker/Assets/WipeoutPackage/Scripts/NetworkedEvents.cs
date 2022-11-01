using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedEvents : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkedEvents Instance;
    public PlayerController LocalPlayer;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        switch (eventCode)
        {
            case 1:
                HandleSpikeEvent((object[])photonEvent.CustomData, photonEvent);
                break;
        }
    }

    public void HandleSpikeEvent(object[] data, EventData photonEvent)
    {
        int lane = (int)data[0];
        if(photonEvent.Sender != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            LocalPlayer.SpawnSpike(ObstacleType.SPIKE, lane);
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
