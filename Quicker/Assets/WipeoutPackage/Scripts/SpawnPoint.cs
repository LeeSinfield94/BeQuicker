using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
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
}
