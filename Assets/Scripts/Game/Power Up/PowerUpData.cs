using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Custom/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string ObjectName; // hierarchy name
    public EPowerUpType Type;
    [Range(0, 15)] public int DurationInSeconds;
    public float Value;
    [Range(0f, 10f)] public float spawnChance;
}