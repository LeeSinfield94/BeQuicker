using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    protected virtual void DoEffect(Player player)
    {
        print("Do Effect");
    }
    protected virtual void UndoEffect(Player player)
    {
        print("Undo Effect");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DoEffect(other.GetComponent<Player>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UndoEffect(other.GetComponent<Player>());
        }
    }
}
