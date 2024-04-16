using UnityEngine;
using System.Collections.Generic;
using MEC;

public class PowerUp : MonoBehaviour
{
    private CoroutineHandle waitHandle;
    private Player playerRef;
    public PowerUpData data;

    public virtual void Collected(GameObject other)
    {
        playerRef = other.GetComponent<Player>();
        if (playerRef == null) return;

        playerRef.ApplyPowerUp(data.Type, data.Value);

        if (waitHandle != null)
        {
            Timing.KillCoroutines(waitHandle);
            Expired();
        }
        waitHandle = Timing.RunCoroutine(WaitPowerUpDuration());
    }
    protected IEnumerator<float> WaitPowerUpDuration()
    {
        yield return Timing.WaitForSeconds(data.DurationInSeconds);

        Expired();
    }
    public virtual void Expired()
    {
        playerRef.ApplyPowerUp(data.Type, -data.Value);
    }
}