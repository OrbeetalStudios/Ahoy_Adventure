using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CrewAssignPanel : MonoBehaviour
{
    [SerializeField] private Transform panelCharacter;
    [SerializeField] private Transform panelAbility;
    [SerializeField] private CrewHire crewHire;
    [SerializeField] Image buttonImagePrefab;
    public List<int> assignID = new List<int>();
   


    private void Start()
    {
        CreateLastCrew();
    }

    public void CreateLastCrew()
    {
        assignID=CrewController.Instance.idAssigned;
        foreach (int id in assignID)
        {
            // Trova il bottone dell'equipaggio corrispondente all'ID assegnato
            CrewData crewButton = null;
            foreach (CrewData button in CrewController.Instance.crewData)
            {
                if (button.characterID == id)
                {
                    crewButton = button;
                    break;
                }
            }

            // Se il bottone è valido, crea il pulsante nell'assegnazione
            if (crewButton != null)
            {
                Image newCharIMG = Instantiate(buttonImagePrefab, panelCharacter);
                Image imageToSet = newCharIMG.GetComponent<Image>();
                Image newAbilityIMG = Instantiate(buttonImagePrefab, panelAbility);
                Image abilityToSet = newAbilityIMG.GetComponent<Image>();

                // Imposta gli sprite direttamente dai bottoni dell'equipaggio
                imageToSet.sprite = crewButton.sprite;
                abilityToSet.sprite = crewButton.AbilitySprite;
            }
        }
    }


    public void Assign(int characterID, Sprite sprite, Sprite ability)
    {// Controlla se l'ID è già presente nella lista assignID
        if (!assignID.Contains(characterID)&&assignID.Count<4)
        {
            // Se l'ID non è già presente, aggiungilo alla lista e crea il pulsante
            Image newCharIMG = Instantiate(buttonImagePrefab, panelCharacter);
            Image imageToSet = newCharIMG.GetComponent<Image>();
            Image newAbilityIMG = Instantiate(buttonImagePrefab, panelAbility);
            Image abilityToSet = newAbilityIMG.GetComponent<Image>();
            imageToSet.sprite = sprite;
            abilityToSet.sprite = ability;
            assignID.Add(characterID);
        }
    }

    public void Dismiss(int idToDismiss)
    {
        int indexToRemove = assignID.IndexOf(idToDismiss);
        if (indexToRemove != -1)
        {
            // Rimuovi il pulsante e l'immagine associata dalla gerarchia degli oggetti
            Destroy(panelCharacter.GetChild(indexToRemove).gameObject);
            Destroy(panelAbility.GetChild(indexToRemove).gameObject);

            // Rimuovi l'id dalla lista
            assignID.RemoveAt(indexToRemove);
        }
    }

   
}
