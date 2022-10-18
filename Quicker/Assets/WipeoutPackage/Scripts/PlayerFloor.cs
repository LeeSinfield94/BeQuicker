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
        Vector3 offset;
        GameObject go = ObjectPooler.instance.GetObject(type, this, out offset);
        go.transform.SetParent(lanes[laneIndex]);
        go.transform.position = lanes[laneIndex].position + offset;
        go.SetActive(true); 
    }
}
