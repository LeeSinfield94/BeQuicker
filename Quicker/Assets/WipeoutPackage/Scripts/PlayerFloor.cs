using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private List<Transform> lanes = new List<Transform>();
    public List<Transform> Lanes
    {
        get { return lanes; }
    }

    public void SpawnObstacleOnFloor(ObstacleType type, int laneIndex)
    {
        GameObject go = ObjectPooler.instance.GetObject(type, this);
        go.transform.SetParent(lanes[laneIndex]);
        go.SetActive(true); 
    }

}
