using UnityEngine;
using System.Collections.Generic;
using MEC;

public class PowerUp : MonoBehaviour
{
    private CoroutineHandle waitHandle;
    protected Player playerRef;
    [SerializeField] protected float DurationInSeconds = 5;

    public virtual void Collected(GameObject other)
    {
        playerRef = other.GetComponent<Player>();

        waitHandle = Timing.RunCoroutine(WaitPowerUpDuration());
    }
    protected IEnumerator<float> WaitPowerUpDuration()
    {
        yield return Timing.WaitForSeconds(DurationInSeconds);

        Expired();
    }
    public virtual void Expired()
    {
        Debug.Log("Default PowerUpExpired method!");
    }
}