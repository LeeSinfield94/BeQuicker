using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ObjectsToSpawn
{
    public int amountToSpawn;
    public GameObject objectToSpawn;
    //public Transform parent;
}
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private List<ObjectsToSpawn> objectsToSpawn = new List<ObjectsToSpawn>();

    public List<GameObject> spikeObjects = new List<GameObject>();
    public static ObjectPooler instance;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            SpawnTraps();
        }

    }
    public void AddToList(GameObject go)
    {
        spikeObjects.Add(go);
    }

    public void SpawnTraps()
    {
        foreach (ObjectsToSpawn objectToSpawn in objectsToSpawn)
        {
            for (int i = 0; i < objectToSpawn.amountToSpawn; i++)
            {
                GameObject go = PhotonNetwork.Instantiate(objectToSpawn.objectToSpawn.name, Vector3.zero, Quaternion.identity);
                AddToList(go);
                go.SetActive(false);
            }
        }

        GameObject.FindObjectOfType<PlayerController>().CallUpdateTrapObjects();
    }

    public void GetAllSpawnedObjects()
    {
        var spikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach(GameObject go in spikes)
        {
            go.SetActive(false);
            AddToList(go);
        }
    }

    public GameObject GetObject(ObstacleType type, PlayerFloor floor, out Vector3 yOffset)
    {
        switch (type)
        {
            case ObstacleType.SPIKE:
                yOffset = GetSpikeObject(floor).GetComponent<SpikeObstacle>().yOffset;
                return GetSpikeObject(floor);
        }
        yOffset = Vector3.zero;
        return null;
    }

    public GameObject GetSpikeObject(PlayerFloor floor)
    {
        for (int i = 0; i < spikeObjects.Count; i++)
        {
            if (!spikeObjects[i].activeInHierarchy)
            {
                return spikeObjects[i];
            }
        }
        return null;
    }
}
