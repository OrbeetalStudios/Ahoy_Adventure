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
    private Dictionary<CharacterName, CrewData> characterDataMap;
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
        
    }
}