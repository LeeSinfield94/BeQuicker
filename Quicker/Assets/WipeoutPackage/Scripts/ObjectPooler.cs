using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ObjectsToSpawn
{
    public int amountToSpawn;
    public GameObject objectToSpawn;
    public Transform parent;
}
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private List<ObjectsToSpawn> objectsToSpawn = new List<ObjectsToSpawn>();

    public List<GameObject> slowObjects = new List<GameObject>();
    public List<GameObject> spikeObjects = new List<GameObject>();
    public static ObjectPooler instance;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        foreach(ObjectsToSpawn objectToSpawn in objectsToSpawn)
        {
            for(int i = 0; i < objectToSpawn.amountToSpawn; i++)
            {
                GameObject go = Instantiate(objectToSpawn.objectToSpawn, objectToSpawn.parent);
                AddToList(go);
                go.SetActive(false);
            }
        }
    }
    public void AddToList(GameObject go)
    {
        if (go.CompareTag("Slow"))
        {
            slowObjects.Add(go);
        }
        else
        {
            spikeObjects.Add(go);
        }
    }
    public GameObject GetObject(ObstacleType type, PlayerFloor floor)
    {
        switch (type)
        {
            case ObstacleType.SLOW:
                return GetSlowObject(floor);
            case ObstacleType.SPIKE:
                return GetSpikeObject(floor);
        }
        return null;
    }

    public GameObject GetSlowObject(PlayerFloor floor)
    {
        for(int i = 0; i < slowObjects.Count; i++)
        {
            if(!slowObjects[i].activeInHierarchy && slowObjects[i].transform.parent == floor.transform)
            {
                return slowObjects[i];
            }
        }
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
