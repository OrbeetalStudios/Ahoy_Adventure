using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Custom/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string ObjectName; // hierarchy name
    public EPowerUpType Type;
    public Sprite img;
    [Range(0, 60)] public int DurationInSeconds;
    public float Value;
    public bool IsPermanent = false;
    [Range(0f, 10f)] public float spawnChance;

    public void CollectPowerUp()
    {
        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpCollected(this));
        }
    }
    public void ExpirePowerUp()
    {
        // Send message to any listeners
        foreach (GameObject go in EventListener.Instance.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvent>(go, null, (x, y) => x.OnPowerUpExpired(this));
        }
    }
}