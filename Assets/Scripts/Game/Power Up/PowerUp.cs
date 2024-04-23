using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using MEC;

public class PowerUp : MonoBehaviour
{
    private CoroutineHandle waitHandle;
    public PowerUpData data;
    public bool active = false;
    private int currentDurationTime;

    public void Collected()
    {
        currentDurationTime = data.DurationInSeconds; // reset power up duration counter
        if (!waitHandle.IsValid)
        {
            waitHandle = Timing.RunCoroutine(WaitPowerUpDuration().CancelWith(gameObject));
        }

        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpCollected(this.data));
        }     
    }
    protected IEnumerator<float> WaitPowerUpDuration()
    {
        Debug.Log("Activated " + data.ObjectName + "!!!");

        while (currentDurationTime > 0)
        {
            currentDurationTime--;
            yield return Timing.WaitForSeconds(1f);
        }

        Expired();
    }
    private void Expired()
    {
        Debug.Log("Expired " + data.ObjectName + "!!!");

        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpExpired(this.data));
        }
    }
}