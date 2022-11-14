using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPath : MonoBehaviour
{
    [SerializeField] PlayerFloor _parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _parent.MoveFloorToPlayerLocation();
        }
    }
}
