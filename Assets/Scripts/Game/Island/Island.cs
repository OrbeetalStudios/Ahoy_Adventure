using UnityEngine;

public class Island : MonoSingleton<Island>
{
    [SerializeField, Range(1, 10)] private float currentTreasure;
    public float CurrentTreasure { get { return currentTreasure; } }
    [SerializeField] private int treasureLostSfxIndex;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
    }

    public void DecreaseTreasure(float amount)
    {
        PlaySFX(treasureLostSfxIndex);
        currentTreasure -= amount;
        GameController.Instance.UpdateTreasureUI(currentTreasure);
        if (currentTreasure <= 0f)
        {
            //Game over
            GameController.Instance.GameOver();
        }
    }

    public void IncreaseTreasure(float amount)
    {
        currentTreasure += amount;
        GameController.Instance.UpdateTreasureUI(currentTreasure);
    }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}
