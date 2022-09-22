using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowObstacle : BaseObstacle
{
    protected override void DoEffect(PlayerData player)
    {
        print("SlowEffect");
        if (player != null)
        {
            player.SetSlowed(true);
        }
    }

    protected override void UndoEffect(PlayerData player)
    {
        if (player != null)
        {
            player.SetSlowed(false);
        }
    }
}
