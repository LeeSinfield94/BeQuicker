using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    protected virtual void DoEffect(PlayerController player)
    {
        print("Do Effect");
    }
    protected virtual void UndoEffect(PlayerController player)
    {
        print("Undo Effect");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DoEffect(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UndoEffect(other.GetComponent<PlayerController>());
        }
    }
}
