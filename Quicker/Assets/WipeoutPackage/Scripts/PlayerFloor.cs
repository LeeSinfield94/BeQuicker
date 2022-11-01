using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] List<Transform> _lanes = new List<Transform>();
    int _trapDistanceFromPlayer = 15;
    public List<Transform> Lanes
    {
        get { return _lanes; }
    }

    public void SpawnObstacleOnFloor(ObstacleType type, int laneIndex, PlayerController player)
    {
        Vector3 yOffset;
        GameObject go = ObjectPooler.Instance.GetObject(type, this, out yOffset);
        go.transform.SetParent(_lanes[laneIndex]);
        Vector3 pos = _lanes[laneIndex].position + yOffset;
        go.transform.position = new Vector3(pos.x, pos.y, pos.z + player.transform.position.z + _trapDistanceFromPlayer);
        go.SetActive(true); 
    }
}
