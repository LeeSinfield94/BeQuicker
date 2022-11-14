using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : BaseObstacle
{
    public Vector3 YOffset;

    protected override void DoEffect(PlayerController player)
    {
        if(player != null)
        {
            player.ModifyHealth(false, 5f);
        }
    }
}
