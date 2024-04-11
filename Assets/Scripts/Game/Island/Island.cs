using UnityEngine;

public class Island : MonoSingleton<Island>
{
    [SerializeField, Range(1, 30)] private int maxTreasure;
    [ShowOnly] [SerializeField] private int currentTreasure;
    // Start is called before the first frame update
    void Start()
    {
        currentTreasure = maxTreasure;
    }

    public void DecreaseTreasure(int amount = 1)
    {
        currentTreasure -= amount;

        if (currentTreasure <= 0)
        {
            //Game over
            GameController.Instance.GameOver();
        }
    }
}
