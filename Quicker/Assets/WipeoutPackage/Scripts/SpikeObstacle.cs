using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : BaseObstacle
{
    protected override void DoEffect(Player player)
    {
        if(player != null)
        {
            player.CanMoveForward = false;
        }
    }

    protected override void UndoEffect(Player player)
    {
        if (player != null)
        {
            player.CanMoveForward = true;
        }
    }
}
