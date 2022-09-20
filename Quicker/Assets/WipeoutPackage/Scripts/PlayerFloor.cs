using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] private ObstacleType obstacleType;
    private Player myPlayer;
    
    public void SpawnObstacleOnFloor(ObstacleType type)
    {
        GameObject go = ObjectPooler.instance.GetObject(type, this);
        go.SetActive(true); 
    }
}
