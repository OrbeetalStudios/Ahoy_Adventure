using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewPanelSolo : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text bioText;
    public TMP_Text abilityText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private CrewButtonsPanel crewButtonsPanel;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image abilitySprite;
    [SerializeField] private Image characterSprite;
    [SerializeField] private GameObject choosePirate;

    private void Update()
    {
        // Assicurati che ci sia almeno un bottone attivo all'avvio
        if (crewButtonsPanel.activeButton != null)
        {
            choosePirate.SetActive(false);
            // Imposta il personaggio attivo quando il pannello viene avviato
            SetActiveCharacter(crewButtonsPanel.activeButton);
        }
    }

    public void SetActiveCharacter(Button activeButton)
    {
        // Ottieni i dati del personaggio dal bottone attivo
        if (activeButton != null)
        {
            CrewButton crewButton = activeButton.GetComponent<CrewButton>();
            if (crewButton != null)
            {
                // Applica i dati del personaggio ai testi delle descrizioni
                nameText.text = crewButton.nameCharacter;
                bioText.text = crewButton.bio;
                abilityText.text = crewButton.ability;
                categoryText.text = crewButton.category;
                costText.text = crewButton.cost.ToString();
                abilitySprite.sprite = crewButton.abilitySprite;
                characterSprite.sprite = crewButton.characterSprite;
            }

            else
            {
                Debug.LogError("CrewButton component missing on active button!");
            }
        }
    }

    public void OnClickCharacter()
    {
        AudioManager.Instance.PlaySpecificOneShot(0);
    }
}
