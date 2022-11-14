using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    [SerializeField] List<Transform> _lanes = new List<Transform>();
    public List<Transform> Lanes
    {
        get { return _lanes; }
    }

    Vector3 _startPos;
    int _trapDistanceFromPlayer = 15;
    int _floorRepositionAmount = 20;


    private void OnDisable()
    {
        GameManager.Instance.LevelRestarting -= LevelRestart;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _startPos = transform.position;
        GameManager.Instance.LevelRestarting += LevelRestart;
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

    public void MoveFloorToPlayerLocation()
    {
        Vector3 newPosition = transform.position;
        newPosition.z += _floorRepositionAmount;
        transform.position = newPosition;
    }

    public void LevelRestart()
    {
        transform.position = _startPos;
    }
}
