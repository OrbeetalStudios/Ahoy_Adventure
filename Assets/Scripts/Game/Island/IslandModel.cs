using UnityEngine;

public class IslandModel : ModelController, IPowerUpEvent
{
    private int islandLevel;

    private void OnEnable()
    {
        islandLevel = 0;

        UpdateIslandModel();
    }
    private void Start()
    {
        base.Start();

        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);
    }
    private void UpdateIslandModel()
    {
        if (islandLevel < 0 || islandLevel >= models.Count) return;

        for (int i = 0; i < models.Count; i++)
        {
            models[i].SetActive(i == islandLevel);
        }
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        if (data.Type != EPowerUpType.IslandLevelUp) return;

        islandLevel += (int)data.Value;

        UpdateIslandModel();
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        // nothing
    }
}