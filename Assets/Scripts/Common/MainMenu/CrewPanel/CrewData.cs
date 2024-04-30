using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrewData", menuName = "Custom/CrewCharacter")]

public class CrewData : ScriptableObject
{
    public int characterID;
    public string characterName;
    public string ability;
    public string bio;
    public Sprite sprite;
    public int Cost;
    public Sprite AbilitySprite;
    public Sprite buttonSprite;
    public Sprite buttonSpriteLight;
    public float value;
    public enum CategoryID
    {
        Normal,
        Special,
        Legend
    }

    public enum PowerUp
    {
        speed,
        life,
        doubloons,
        HPTresure,
    }

    public PowerUp powerUp;

    public CategoryID Category;
}

