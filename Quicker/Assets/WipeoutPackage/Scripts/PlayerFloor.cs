using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private List<Transform> lanes = new List<Transform>();
    private int trapDistanceFromPlayer = 15;
    public List<Transform> Lanes
    {
        get { return lanes; }
    }

    public void SpawnObstacleOnFloor(ObstacleType type, int laneIndex, PlayerData player)
    {
        Vector3 yOffset;
        GameObject go = ObjectPooler.instance.GetObject(type, this, out yOffset);
        go.transform.SetParent(lanes[laneIndex]);
        Vector3 pos = lanes[laneIndex].position + yOffset;
        go.transform.position = new Vector3(pos.x, pos.y, pos.z + player.transform.position.z + trapDistanceFromPlayer);
        go.SetActive(true); 
    }
}
