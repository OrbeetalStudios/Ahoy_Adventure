using UnityEngine;
using System.Collections.Generic;
using MEC;

public class PowerUp : MonoBehaviour
{
    public PowerUpData data;
    [SerializeField] private bool active = false;
    [SerializeField] private int currentDurationTime;

    public void Collected()
    {
        currentDurationTime = data.DurationInSeconds; // reset power up duration counter
        if (active) return; // if already collected, do not send the signal again. Only reset the duration counter

        if (!data.IsPermanent)
        {
            Timing.RunCoroutine(WaitPowerUpDuration().CancelWith(gameObject));
        }

        data.CollectPowerUp();    
    }
    protected IEnumerator<float> WaitPowerUpDuration()
    {
        active = true;
        while (currentDurationTime > 0)
        {
            currentDurationTime--;
            yield return Timing.WaitForSeconds(1f);
        }

        data.ExpirePowerUp();
        active = false;
    }
}