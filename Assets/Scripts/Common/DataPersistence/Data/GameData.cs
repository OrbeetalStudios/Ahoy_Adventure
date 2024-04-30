using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{
    public int doubloons;
    public int score1;
    public int score2;
    public int score3;
    public List<int> idPurchased;
    public List<int> idAssigned;

    public GameData()
    {
        this.doubloons = 200;
        this.score1 = 0;
        this.score2 = 0;
        this.score3 = 0;
        idPurchased = new List<int>();
        idAssigned = new List<int>();
    }
}
