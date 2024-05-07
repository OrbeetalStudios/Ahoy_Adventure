using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoSingleton<PowerUpController>
{
    [SerializeField] private List<PowerUpData> dataList;
    public List<PowerUpData> DataList { get { return dataList; } }
    private List<PowerUp> powerUps = new();
    private float[] weights;

    private void Start()
    {
        weights = new float[dataList.Count];

        PowerUpData data;
        for (int i = 0; i < dataList.Count; i++)
        {
            data = dataList[i];

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
    public int ActivateRandomPowerUp()
    {
        int index = WeightedRandom.GetRandomWeightedIndex(weights);
        Activate(index);
        return index;    
    }
    public void ActivatePowerUp(EPowerUpType powerUpType)
    {
        Activate(powerUps.FindIndex(x => x.data.Type == powerUpType));
    }
    private void Activate(int index)
    {
        if (index >= 0 && index < powerUps.Count)
        {
            powerUps[index].Collected();
        }
    }
}