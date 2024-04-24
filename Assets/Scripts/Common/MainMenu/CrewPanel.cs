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
    [SerializeField] private int Doubloons=50;
    private int cost;
    private Dictionary<CharacterName, CrewData> characterDataMap;
    private int selectedIndex = 0;
    private List<CharacterName> hiredCharacters = new List<CharacterName>(); // Lista per tenere traccia dei personaggi acquistati
    private CharacterName currentCharacter; // Personaggio corrente
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

        // Cerca l'indice selezionato nella lista
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

        // Imposta i dati del personaggio nelle variabili appropriate
        nameText.text = data.characterName;
        Bio.text = data.bio;
        Ability.text = data.ability;
        characterSprite.sprite = data.sprite;
        characterAbility.sprite = data.AbilitySprite;
        cost = data.Cost;
        if (hiredCharacters.Contains(currentCharacter))
        {
            hireButton.sprite= selectionSprite[1];
            hireButtonText.text = "Assigned";
        }
        else
        {
            hireButton.sprite = selectionSprite[0];
            hireButtonText.text = "hire";
        }
        
    }

    public void OnClickHIRE()
    {

        switch (selectedIndex)
        {
            case 0:
                if (Doubloons >= cost)
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
                
                AudioManager.Instance.PlaySpecificOneShot(4); break;
            case 2:
                hireButton.sprite = selectionSprite[1];
                hireButtonText.text = "Assigned";
                AudioManager.Instance.PlaySpecificOneShot(17);
                break;

            default:
                hireButton.sprite= selectionSprite[0];
                hireButtonText.text = "Hire";
                selectedIndex=0;
                break;
        }

    }

    private void BuyPirate()
    {
        //aggiornaidoblooni
        //togliPannelloCosto
        Doubloons -= cost;
    }
}