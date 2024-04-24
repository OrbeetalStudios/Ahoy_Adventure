using UnityEngine;

public class Island : MonoSingleton<Island>
{
    [SerializeField, Range(1, 30)] private int maxTreasure;
    [ShowOnly] [SerializeField] private int currentTreasure;
    public int CurrentTreasure { get { return currentTreasure; } }
    [SerializeField] private int treasureLostSfxIndex;
    // Start is called before the first frame update
    void Awake()
    {
        currentTreasure = maxTreasure;
    }

   public void DecreaseTreasure(int amount = 1)
   {
        PlaySFX(treasureLostSfxIndex);
        // commentato al momento
        //currentTreasure -= amount;
   }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}
