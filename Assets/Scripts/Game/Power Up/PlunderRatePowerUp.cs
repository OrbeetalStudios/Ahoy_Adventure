using UnityEngine;

public class PlunderRatePowerUp : PowerUp
{
    [SerializeField] private float plunderRateBoost = 20;

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
