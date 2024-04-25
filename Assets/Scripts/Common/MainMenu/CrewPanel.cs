using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CrewPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text doubloonsAmount;
    [SerializeField] private GameObject chooseText;
    [SerializeField] private GameObject allPanelCrew;
    [SerializeField] private Transform assagnedCharacterPanel;
    [SerializeField] private Transform assagnedCharacterAbility;
    [SerializeField] private GameObject soloCrewPanel;
    [SerializeField] private Transform VerticalButton; 
    [SerializeField] private Button characters;
    [SerializeField] private TMP_Text Bio;
    [SerializeField] private GameObject panelCost;
    [SerializeField] private Animation anim;
    [SerializeField] private TMP_Text Ability;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image imageButtonSolo;
    [SerializeField] private Image imageButtonCrew;
    [SerializeField] private List<CrewData> characterDataList;
    [SerializeField] private Image characterAbility;
    public List<Sprite> selectionSprite;
    [SerializeField] private Image hireButton;
    [SerializeField] private TMP_Text hireButtonText;
    private CrewData currentCharacterData;
    [SerializeField] private int Doubloons=200;
    private bool choosePirate;
    private int cost;
    private bool purchased;
    private Dictionary<string, CrewData> characterDataMap;
    private int selectedIndex = 0;
    public Button characterButtonPrefab;
    private Button buttonSelected;
    private Sprite previousSpriteDefault;
    private List<Button> newButtonCrew = new List<Button>();
    private List<Image> newAbilityCrew = new List<Image>();



    [ProButton]
    public void ResetValuteTest()
    {
        foreach (CrewData characterData in characterDataList)
        {
            // Resetta il valore di purchased a false
            characterData.purchased = false;

            // Resetta il valore di lastIndex a 0
            characterData.lastIndex = 0;
        }

        // Resetta il valore di Doubloons al suo valore iniziale (40)
        Doubloons = 200;
    }
    
    private void Start()
    {

        doubloonsAmount.text = Doubloons.ToString();
        characterDataMap = new Dictionary<string, CrewData>();
        CreateButtons(characterDataMap);
        // Applica gli sprite ad ogni indice
        for (int i = 0; i < selectionSprite.Count; i++)
        {
            if (selectionSprite[i] == hireButton.sprite)
            {
                selectedIndex = i;
                break;
            }
        }
        ResetValuteTest();

    }

    public void CreateButtons(Dictionary<string, CrewData> characterDataMap)
    {
        foreach (CrewData characterData in characterDataList)
        {
            characterDataMap[characterData.characterName] = characterData;
        }
        // Loop attraverso i dati dei personaggi per creare dinamicamente i bottoni
        foreach (var characterEntry in characterDataMap)
        {
            string characterName = characterEntry.Key;
            CrewData characterData = characterEntry.Value;

            // Crea un nuovo oggetto bottone
            GameObject newButtonObject = new GameObject(characterName.ToString());
            Button newButton = newButtonObject.AddComponent<Button>();

            // Aggiungi il pulsante alla UI
            newButton.transform.SetParent(VerticalButton, false);
            
            // Assegna gli sprite al pulsante
            Sprite buttonSprite = characterData.buttonSprite;
            Sprite buttonSpriteLight = characterData.buttonSpriteLight;
            Image buttonImage = newButtonObject.AddComponent<Image>();
            buttonImage.sprite=buttonSprite;
            newButton.targetGraphic = buttonImage;
            // Configura la transizione del pulsante
            newButton.transition = Selectable.Transition.SpriteSwap;
            SpriteState spriteSwapState = new SpriteState
            {
                highlightedSprite = buttonSpriteLight,
                selectedSprite = buttonSpriteLight 
            };
            newButton.spriteState = spriteSwapState;
            newButton.onClick.AddListener(() => OnButtonClick(characterName, buttonImage, buttonSpriteLight,newButton));        }
    }

    public void OnButtonClick(string characterName, Image buttonImage , Sprite buttonSpriteLight, Button newButton)
    {

        if (buttonSelected != newButton)
        {
            // Se c'è già un pulsante selezionato diverso da quello attualmente cliccato
            if (buttonSelected != null)
            {
                // Ripristina l'immagine precedente del pulsante selezionato
                buttonSelected.image.sprite = previousSpriteDefault;
            }

            // Memorizza il nuovo pulsante selezionato e la sua immagine predefinita
            buttonSelected = newButton;
            previousSpriteDefault = buttonImage.sprite;
        }

        // Modifica l'immagine del pulsante attualmente cliccato
        buttonImage.sprite = buttonSpriteLight;


        AudioManager.Instance.PlaySpecificOneShot(4);
        chooseText.SetActive(false);
        choosePirate = true;
        // Ottieni i dati del personaggio dal dizionario
        CrewData data = characterDataMap[characterName];
        currentCharacterData = data;
        // Imposta i dati del personaggio nelle variabili appropriate
        nameText.text = data.characterName;
        Bio.text = data.bio;
        Ability.text = data.ability;
        characterSprite.sprite = data.sprite;
        characterAbility.sprite = data.AbilitySprite;
        categoryText.text = data.Category.ToString();
        cost = data.Cost;
        costText.text = cost.ToString();
        SetHireButtonDefault(currentCharacterData);
        
    }

    public void OnClickHIRE()
    {
        if (!choosePirate)
        {
            AudioManager.Instance.PlaySpecificOneShot(19);
        }
        else
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
                        anim.Play();
                        AudioManager.Instance.PlaySpecificOneShot(19);
                    }
                    break;
                case 1:
                    if (newButtonCrew.Count == 4)
                    {
                        AudioManager.Instance.PlaySpecificOneShot(19);
                    }
                    else
                    {
                        AssignedPirates(currentCharacterData);
                        hireButton.sprite = selectionSprite[2];
                        hireButtonText.text = "Dismiss";
                        currentCharacterData.lastIndex = 2;
                        AudioManager.Instance.PlaySpecificOneShot(18); 
                    }
                    break;

                case 2:
                    DismissPirate();
                    hireButton.sprite = selectionSprite[1];
                    hireButtonText.text = "Assigned";
                    currentCharacterData.lastIndex = 1;
                    AudioManager.Instance.PlaySpecificOneShot(17);
                    break;

                default:
                    hireButton.sprite = selectionSprite[0];
                    hireButtonText.text = "Hire";
                    break;
            }
        }
    }

    private void BuyPirate()
    {
        panelCost.SetActive(false);
        currentCharacterData.purchased = true;
        currentCharacterData.lastIndex = 1;
        Doubloons -= cost;
        doubloonsAmount.text = Doubloons.ToString();

    }

    public void OnClickCharacter()
    {
        if (choosePirate)
        {
            AudioManager.Instance.PlaySpecificOneShot(0);
        }
       
    }

    public void EnableAllCrewPanel()
    {
        allPanelCrew.SetActive(true); 
        soloCrewPanel.SetActive(false);
    }
    public void DisableAllCrewPanel()
    {
        allPanelCrew.SetActive(false);
        soloCrewPanel.SetActive(true);
    }

    //cercare di riciclare i pulsanti
    private void AssignedPirates(CrewData currentCharacterData)
    {
        // Crea una copia del prefab del pulsante del personaggio
        characterButtonPrefab = buttonSelected;
        Sprite currentSprite= currentCharacterData.AbilitySprite;
        // Istanzia un'immagine e imposta lo sprite
        GameObject newObjImageObject = new GameObject("CharacterAbilityImage");
        Image newObjImage = newObjImageObject.AddComponent<Image>();
        newAbilityCrew.Add(newObjImage);
        newObjImage.sprite = currentSprite;
        newObjImage.transform.SetParent(assagnedCharacterAbility, false);
        Button newObjButton = Instantiate(characterButtonPrefab, assagnedCharacterPanel);
        newButtonCrew.Add(newObjButton);
        
    }
    private void DismissPirate()
    {
        for (int i = 0; i < newButtonCrew.Count; i++)
        {
            // Controlla se le immagini dei pulsanti corrispondono
            if (newButtonCrew[i].image.sprite == buttonSelected.image.sprite)
            {
                // Rimuovi il pulsante dalla gerarchia degli oggetti
                Destroy(newButtonCrew[i].gameObject);
                Destroy(newAbilityCrew[i].gameObject);

                // Rimuovi il pulsante dalla lista
                newButtonCrew.RemoveAt(i);
                newAbilityCrew.RemoveAt(i);
                break; // Esci dal ciclo una volta trovato il pulsante
            }

        }

    }

    private void SetHireButtonDefault(CrewData currentCharacterData)
    {
        if (currentCharacterData.purchased == true)
        {
            panelCost.SetActive(false);
            if (currentCharacterData.lastIndex == 1)
            {
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
            panelCost.SetActive(true);
            hireButton.sprite = selectionSprite[0];
            hireButtonText.text = "hire";
        }

    }
  

}