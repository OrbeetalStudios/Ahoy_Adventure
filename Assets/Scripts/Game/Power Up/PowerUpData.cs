using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Custom/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string ObjectName;
    public EPowerUpType Type;
    public float DurationInSeconds = 5;
    public float Value;
}