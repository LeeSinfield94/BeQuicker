using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public SpawnPoint()
    {
        CanSpawn = true;
    }
    public bool CanSpawn
    {
        get;
        set;
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.transform.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            player.MyFloor = GetComponentInParent<PlayerFloor>();
        }
    }
}
