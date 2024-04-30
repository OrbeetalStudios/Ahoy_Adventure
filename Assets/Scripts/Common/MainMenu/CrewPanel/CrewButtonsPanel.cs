using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class CrewButtonsPanel : MonoBehaviour
{
    public List<CrewData> CrewData;
    public GameObject buttonPrefab;
    public Transform buttonContainer; // Riferimento al contenitore dei bottoni nel pannello
    public Button activeButton;
    public CrewButton crewButton;
    public CrewHire hire;
   
   
    private void Start()
    {
        buttonContainer=transform;
        CreateButtons();
    }
  

    public void CreateButtons()
    {
        foreach (var crewData in CrewData)
        {
            // Istanzia il prefab del bottone
            GameObject newButtonObject = Instantiate(buttonPrefab, buttonContainer);

            // Ottieni il componente CrewButton dal nuovo bottone
            crewButton = newButtonObject.GetComponent<CrewButton>();

            // Se il componente CrewButton esiste, imposta i dati della CrewData
            if (crewButton != null)
            {
                crewButton.SetCrewData(crewData);//Non gestisce l'indice e il purchased 
            }
            else
            {
                Debug.LogError("CrewButton component is missing on button prefab!");
            }
            Button newButton = newButtonObject.GetComponent<Button>();

            // Aggiungi un listener per il click del bottone
            newButton.onClick.AddListener(() => OnButtonClick(newButton));
        }

    }

    private void OnButtonClick(Button newButton)
    {
        AudioManager.Instance.PlaySpecificOneShot(4);
        // Rimuovi il bottone attualmente attivo dalla lista
        if (activeButton != null)
        {
            CrewButton prevCrewButton = activeButton.GetComponent<CrewButton>();
            if (prevCrewButton != null)
            {
                // Aggiorna i dati del bottone precedente
                hire.SetButton(prevCrewButton);
            }
            Button prevButton = activeButton;
            prevButton.interactable = true; // Riattiva il bottone precedente
            Image prevButtonImage = prevButton.GetComponent<Image>();
            if (prevButtonImage != null)
            {
                // Reimposta lo sprite predefinito al bottone precedente
                prevButtonImage.sprite = prevCrewButton.buttonDefault;
            }
        }

        // Aggiorna il nuovo bottone alla lista dei bottoni attivi
        activeButton = newButton;
        // Disattiva il bottone appena cliccato
        newButton.interactable = false;

        // Applica lo sprite di luce al bottone cliccato
        Image buttonImage = newButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = newButton.GetComponent<CrewButton>().buttonLight;
        }
        CrewButton activeCrewButton = newButton.GetComponent<CrewButton>();
        hire.SetButton(activeCrewButton);
    }
}