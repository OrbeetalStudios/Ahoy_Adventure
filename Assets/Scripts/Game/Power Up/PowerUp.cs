using UnityEngine;
using UnityEngine.EventSystems;
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
        if (!active && !data.IsOneShot)
        {
            Timing.RunCoroutine(WaitPowerUpDuration().CancelWith(gameObject));
        }

        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpCollected(this.data));
        }     
    }
    protected IEnumerator<float> WaitPowerUpDuration()
    {
        active = true;
        while (currentDurationTime > 0)
        {
            currentDurationTime--;
            yield return Timing.WaitForSeconds(1f);
        }

        Expired();
        active = false;
    }
    private void Expired()
    {
        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpExpired(this.data));
        }
    }
}