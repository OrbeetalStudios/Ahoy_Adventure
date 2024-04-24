using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrewPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private List<Button> crewButtonList; // Changed the type to UnityEngine.UI.Button
    [SerializeField] private Button characters;
    [SerializeField] private TMP_Text Bio;
    [SerializeField] private TMP_Text Ability;
    [SerializeField] private Image characterSprite;
    [SerializeField] private List<CrewData> characterDataList;
    [SerializeField] private Image characterAbility;
    public List<Sprite> selectionSprite;
    [SerializeField] private Image hireButton;
    [SerializeField] private TMP_Text hireButtonText;
    private CrewData currentCharacterData;
    [SerializeField] private int Doubloons=40;
    private int cost;
    private bool purchased;
    private Dictionary<CharacterName, CrewData> characterDataMap;
    private int selectedIndex = 0;

    public enum CharacterName
    {
        Peppino,
        Ravanello,
        Frytatina,
        Spritz,
        DonPichotte,
        Fonteena,
        Keith,
        Mort,
        SlimJim
    }

    private void Start()
    {
        characterDataMap = new Dictionary<CharacterName, CrewData>();
        for (int i = 0; i < characterDataList.Count; i++)
        {
            characterDataMap[(CharacterName)i] = characterDataList[i];
        }
        // Loop through each crew button and assign an enum value
        for (int i = 0; i < crewButtonList.Count; i++)
        {
            // Get the corresponding enum value based on the index
            CharacterName character = (CharacterName)i;

            // Assign a click event to the button and pass the enum value
            int index = i; // Store the index in a local variable to avoid closure issues
            crewButtonList[i].onClick.AddListener(() => OnButtonClick(character));
        }

        // Applica gli sprite ad ogni indice
        for (int i = 0; i < selectionSprite.Count; i++)
        {
            if (selectionSprite[i] == hireButton.sprite)
            {
                selectedIndex = i;
                break;
            }
        }

    }

    public void OnButtonClick(CharacterName character)
    {
        // Ottieni i dati del personaggio dal dizionario
        CrewData data = characterDataMap[character];
        currentCharacterData = data;
        // Imposta i dati del personaggio nelle variabili appropriate
        nameText.text = data.characterName;
        Bio.text = data.bio;
        Ability.text = data.ability;
        characterSprite.sprite = data.sprite;
        characterAbility.sprite = data.AbilitySprite;
        cost = data.Cost;
       
        if (data.purchased==true)
        {
            if (currentCharacterData.lastIndex == 1)
            {
                //Ilcostononvienemostrato
                hireButton.sprite = selectionSprite[1];
                hireButtonText.text = "Assigned";
            }
            else
            {
                hireButton.sprite = selectionSprite[2];
                hireButtonText.text = "Dimsiss";
            }
     
        }
        else
        {
            hireButton.sprite = selectionSprite[0];
            hireButtonText.text = "hire";
        }
        
    }

    public void OnClickHIRE()
    {

        switch (currentCharacterData.lastIndex)
        {
            case 0:
                if (Doubloons >= cost && !purchased)
                {
                    BuyPirate();
                    hireButton.sprite = selectionSprite[1];
                    hireButtonText.text = "Assigned";
                    AudioManager.Instance.PlaySpecificOneShot(10);
                    
                }
                else
                {
                    //audio che non permette l'acquisto
                    AudioManager.Instance.PlaySpecificOneShot(4);
                }
                break;
            case 1:
                hireButton.sprite = selectionSprite[2];
                hireButtonText.text = "Dismiss";
                currentCharacterData.lastIndex = 2;
                AudioManager.Instance.PlaySpecificOneShot(18); break;
            case 2:
                hireButton.sprite = selectionSprite[1];
                hireButtonText.text = "Assigned";
                currentCharacterData.lastIndex = 1;
                AudioManager.Instance.PlaySpecificOneShot(17);
                break;

            default:
                hireButton.sprite= selectionSprite[0];
                hireButtonText.text = "Hire";
                currentCharacterData.lastIndex = 0;
                break;
        }

    }

    private void BuyPirate()
    {
        //aggiornaidoblooni
        //togliPannelloCosto
        currentCharacterData.purchased = true;
        currentCharacterData.lastIndex = 1;
        Doubloons -= cost;
       
    }
}