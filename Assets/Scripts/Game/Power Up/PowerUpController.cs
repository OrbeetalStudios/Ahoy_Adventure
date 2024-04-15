using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoSingleton<PowerUpController>
{
    [SerializeField] public List<PowerUpData> DataList;
    private List<PowerUp> powerUps = new();
    private void Start()
    {
        foreach (var data in DataList)
        {
            GameObject obj = new GameObject(data.ObjectName, typeof(PowerUp));
            obj.transform.SetParent(this.transform); // just for better hierarchy
            PowerUp powerUp = obj.GetComponent<PowerUp>();
            powerUp.data = data;
            powerUps.Add(powerUp);
        }
    }
    public void ActivatePowerUp(GameObject other)
    {
        powerUps[Random.Range(0, powerUps.Count)].Collected(other);
    }
}