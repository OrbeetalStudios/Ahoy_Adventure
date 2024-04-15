using UnityEngine;

public class FireRatePowerUp : PowerUp
{
    [SerializeField] private float fireRateBoost = 20;

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
