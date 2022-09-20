using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowObstacle : BaseObstacle
{
    protected override void DoEffect(Player player)
    {
        if (player != null)
        {
            player.SetSpeed(true);
        }
    }

    protected override void UndoEffect(Player player)
    {
        if (player != null)
        {
            player.SetSpeed(false);
        }
    }
}
