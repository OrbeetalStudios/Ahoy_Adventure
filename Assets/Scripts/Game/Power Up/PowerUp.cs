using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using MEC;

public class PowerUp : MonoBehaviour
{
    private CoroutineHandle waitHandle;
    public PowerUpData data;
    public bool active = false;

    public virtual void Collected()
    {
        if (waitHandle != null)
        {
            Timing.KillCoroutines(waitHandle);
            Expired();
        }

        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpCollected(this.data));
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
        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpExpired(this.data));
        }
    }
}