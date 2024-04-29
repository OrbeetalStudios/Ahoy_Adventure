using UnityEngine;

public class Island : MonoSingleton<Island>
{
    [SerializeField, Range(1, 30)] private int maxTreasure = 10;
    [SerializeField] private int currentTreasure;
    public int CurrentTreasure { get { return currentTreasure; } }
    [SerializeField] private int treasureLostSfxIndex;
    // Start is called before the first frame update
    void Awake()
    {
        currentTreasure = maxTreasure;
    }

    public void DecreaseTreasure(int amount)
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

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}
