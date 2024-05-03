using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrewData", menuName = "Custom/CrewCharacter")]

public class CrewData : ScriptableObject
{
    public enum CategoryID
    {
        Normal,
        Special,
        Legend
    }

    public CategoryID Category;
    public int characterID;
    public string characterName;
    public string ability;
    public string bio;
    public Sprite sprite;
    public int Cost;
    public Sprite AbilitySprite;
    public Sprite buttonSprite;
    public Sprite buttonSpriteLight;
    public PowerUpData powerUpData;
}

