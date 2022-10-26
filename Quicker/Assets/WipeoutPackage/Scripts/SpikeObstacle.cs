using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : BaseObstacle
{
    public Vector3 yOffset;

    protected override void DoEffect(PlayerController player)
    {
        if(player != null)
        {
            player.CanMoveForward = false;
        }
    }

    protected override void UndoEffect(PlayerController player)
    {
        if (player != null)
        {
            player.CanMoveForward = true;
        }
    }


}
