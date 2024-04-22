using UnityEngine.EventSystems;

public interface IPowerUpEvent : IEventSystemHandler
{
    void OnPowerUpCollected(PowerUpData data);
    void OnPowerUpExpired(PowerUpData data);
}