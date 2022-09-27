using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, IPunObservable
{
    public SpawnPoint()
    {
        canSpawn = true;
    }
    public bool canSpawn
    {
        get;
        set;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(canSpawn);
        }
        else
        {
            canSpawn = (bool)stream.ReceiveNext();
        }

        Debug.Log($"Can Spawn = {canSpawn}");
    }
}
