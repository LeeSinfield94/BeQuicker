using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private ObstacleType obstacleType;

    public void SpawnObstacleOnFloor(ObstacleType type)
    {
        GameObject go = ObjectPooler.instance.GetObject(type, this);
        go.SetActive(true); 
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name + 1.ToString());
        PlayerData player = other.transform.gameObject.GetComponent<PlayerData>();
        if (player)
        {
            player.MyFloor = this;
        }
    }
}
