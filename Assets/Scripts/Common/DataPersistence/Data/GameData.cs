using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{
    public int doubloons;
    public int[] score;
    public List<int> idPurchased;
    public List<int> idAssigned;

    public GameData()
    {
        this.doubloons = 1000;
        this.score=new int[3];
        idPurchased = new List<int>();
        idAssigned = new List<int>();
    }
}
