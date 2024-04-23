using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoSingleton<PowerUpController>
{
    [SerializeField] public List<PowerUpData> DataList;
    private List<PowerUp> powerUps = new();
    private float[] weights;
    private void Start()
    {
        weights = new float[DataList.Count];

        PowerUpData data;
        for (int i = 0; i < DataList.Count; i++)
        {
            data = DataList[i];

            // Setup the PowerUp game object
            GameObject obj = new GameObject(data.ObjectName, typeof(PowerUp));
            obj.transform.SetParent(this.transform); // just for better hierarchy
            PowerUp powerUp = obj.GetComponent<PowerUp>();
            powerUp.data = data;

            // save useful data
            weights[i] = data.spawnChance;
            powerUps.Add(powerUp);
        }
    }
    public void ActivatePowerUp()
    {
        int index = WeightedRandom.GetRandomWeightedIndex(weights);
        if (index >= 0 && index < powerUps.Count)
        {
            powerUps[index].Collected();
        }
    }
}