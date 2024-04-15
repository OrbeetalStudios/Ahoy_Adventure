using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePowerUp : PowerUp
{
    [SerializeField] private float scoreGoldBoost = 1;

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