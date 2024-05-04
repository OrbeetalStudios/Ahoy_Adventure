
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewButton : MonoBehaviour
{
    public int characterID;
    public string nameCharacter;
    public string bio;
    public string ability;
    public string category;
    public int cost;
    public bool purchased;
    public int lastIndex;
    public Sprite abilitySprite;
    public Sprite characterSprite;
    public Sprite buttonDefault;
    public Sprite buttonLight;
    public Button prefab;
    public MainMenu mainMenu;   
   

    
   

    public void SetCrewData(CrewData data)
    {
        characterID = data.characterID;
        nameCharacter = data.characterName;
        bio=data.bio;   
        ability = data.ability; 
        category = data.Category.ToString();
        abilitySprite = data.AbilitySprite;
        cost = data.Cost;
        characterSprite = data.sprite;
        buttonDefault = data.buttonSprite;
        buttonLight = data.buttonSpriteLight;
        SetButtonAppearance();
    }

    
    private void SetButtonAppearance()
    {
        Image buttonImage=GetComponent<Image>();   
        // Aggiorna l'immagine del pulsante con lo sprite del personaggio
        
        if (buttonImage != null)
        {
            buttonImage.sprite = buttonDefault;
        }

        Button buttonComponent = GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.transition = Selectable.Transition.SpriteSwap;
            SpriteState spriteSwapState = new SpriteState
            {
                highlightedSprite = buttonLight,
                selectedSprite = buttonDefault
            };
            buttonComponent.spriteState = spriteSwapState;
        }
    }

   
}
