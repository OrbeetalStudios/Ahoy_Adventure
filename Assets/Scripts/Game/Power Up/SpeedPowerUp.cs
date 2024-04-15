using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SpeedPowerUp : PowerUp
{
    [SerializeField] private float speedBoost = 20;

    public override void Collected(GameObject other)
    {
        base.Collected(other);
        
        //
    }
    public override void Expired()
    {
        //
    }
}