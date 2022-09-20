using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    protected virtual void DoEffect(PlayerData player)
    {
        print("Do Effect");
    }
    protected virtual void UndoEffect(PlayerData player)
    {
        print("Undo Effect");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DoEffect(other.GetComponent<PlayerData>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UndoEffect(other.GetComponent<PlayerData>());
        }
    }
}
