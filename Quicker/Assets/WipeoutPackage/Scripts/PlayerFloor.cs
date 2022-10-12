using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{

    public void SpawnObstacleOnFloor(ObstacleType type)
    {
        GameObject go = ObjectPooler.instance.GetObject(type, this);
        go.transform.SetParent(this.transform);
        go.SetActive(true); 
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerData player = other.transform.gameObject.GetComponent<PlayerData>();
        if (player)
        {
            player.MyFloor = this;
        }
    }
}
