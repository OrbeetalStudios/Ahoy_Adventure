using UnityEngine;

public class Island : MonoSingleton<Island>
{
    [SerializeField, Range(1, 30)] private int maxTreasure;
    [ShowOnly] [SerializeField] private int currentTreasure;
    [SerializeField] private int treasureLostSfxIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentTreasure = maxTreasure;
    }

   public void DecreaseTreasure(int amount = 1)
   {
        PlaySFX(treasureLostSfxIndex);
        currentTreasure -= amount;

        if (currentTreasure <= 0)
        {
            //Game over
            //GameController.Instance.GameOver();
        }
   }

    private void PlaySFX(int index){
        AudioManager.Instance.PlaySpecificOneShot(index);
    }
}
