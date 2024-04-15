using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoSingleton<PowerUpController>
{
    [SerializeField] public List<GameObject> powerUpsPrefabs;
    private List<PowerUp> powerUps = new();
    private void Start()
    {
        foreach (var item in powerUpsPrefabs)
        {
            GameObject obj = Instantiate(item, this.transform);
            powerUps.Add(obj.GetComponent<PowerUp>());
        }
    }
    public void ActivatePowerUp(GameObject other)
    {
        powerUps[Random.Range(0, powerUps.Count)].Collected(other);
    }
}
