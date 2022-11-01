using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ObjectsToSpawn
{
    public int AmountToSpawn;
    public GameObject ObjectToSpawn;
}
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] List<ObjectsToSpawn> _objectsToSpawn = new List<ObjectsToSpawn>();

    public List<GameObject> SpikeObjects = new List<GameObject>();
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
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
        SpikeObjects.Add(go);
    }

    public void SpawnTraps()
    {
        foreach (ObjectsToSpawn objectToSpawn in _objectsToSpawn)
        {
            for (int i = 0; i < objectToSpawn.AmountToSpawn; i++)
            {
                GameObject go = PhotonNetwork.Instantiate(objectToSpawn.ObjectToSpawn.name, Vector3.zero, Quaternion.identity);
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
                yOffset = GetSpikeObject(floor).GetComponent<SpikeObstacle>().YOffset;
                return GetSpikeObject(floor);
        }
        yOffset = Vector3.zero;
        return null;
    }

    public GameObject GetSpikeObject(PlayerFloor floor)
    {
        for (int i = 0; i < SpikeObjects.Count; i++)
        {
            if (!SpikeObjects[i].activeInHierarchy)
            {
                return SpikeObjects[i];
            }
        }
        return null;
    }
}
