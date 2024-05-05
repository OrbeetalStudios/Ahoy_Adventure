using UnityEngine;
using MEC;
using System.Collections.Generic;

public class Island : MonoSingleton<Island>, IPowerUpEvent
{
    [SerializeField, Range(1, 10)] private float maxTreasure = 10;
    [SerializeField, Range(1, 10)] private float startTreasure = 5;
    private float currentTreasure;
    public float CurrentTreasure { get { return currentTreasure; } }
    public float MaxTreasure { get { return maxTreasure; } }
    [SerializeField] private int treasureLostSfxIndex;

    void Awake()
    {
        base.Awake();

        currentTreasure = startTreasure;
    }
    private void Start()
    {
        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);
    }
    public void DecreaseTreasure(float amount)
    {
        PlaySFX(treasureLostSfxIndex);
        currentTreasure -= amount;
    }
    public void IncreaseTreasure(float amount)
    {
        if (currentTreasure == maxTreasure) return;

        currentTreasure += amount;
    }
    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        if (data.Type != EPowerUpType.HPTreasure) return;

        Timing.RunCoroutine(TimedTreasureIncrease(data.Value).CancelWith(gameObject));
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        // nothing
    }
    private IEnumerator<float> TimedTreasureIncrease(float scoreCheck)
    {
        int old = GameController.Instance.CurrentScore;
        while (true)
        {
            if ((GameController.Instance.CurrentScore - old) >= (int)scoreCheck)
            {
                old = GameController.Instance.CurrentScore;
                IncreaseTreasure(1);
            }
            
            yield return Timing.WaitForOneFrame;
        }   
    }
}