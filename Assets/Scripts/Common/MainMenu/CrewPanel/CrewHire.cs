using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CrewHire : MonoBehaviour
{
    [SerializeField]
    private Button hireButton;
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private GameObject costObj;
    [SerializeField]
    private CrewPanelSolo crewPanelSolo;
    [SerializeField]
    private CrewButtonsPanel crewButtonsPanel;
    [SerializeField]
    private TMP_Text doubloonsText;
    [SerializeField]
    private Image Image;
    [SerializeField]
    private TMP_Text text;
    public CrewButton active;
    private int cost;
    [SerializeField] private Animation anim;
    public CrewAssignPanel crewPanel;
    [SerializeField]
    private MainMenu mainMenu;
    private int characterID;
    public List<int> idPurchased = new List<int>();
    public List<int> idAssigned = new List<int>();
    
    //VARIABILI DA DECIDERE
    public int doubloons;
    private bool purchased;
    private int lastIndex;

    private void OnEnable()
    {
        idPurchased=CrewController.Instance.idPurchased;
        idAssigned= CrewController.Instance.idAssigned;
        doubloons = CrewController.Instance.doubloons;//I dobloni saranno gestiti da MainMenu
        doubloonsText.text = doubloons.ToString();//Applica la scritta hai dobloni
    }
    public void SetButton(CrewButton currentButton)
    {
        active=currentButton;
        characterID=active.characterID;
        cost = active.cost;
        SetCurrentIndex(characterID);
        
    }

    public void SetCurrentIndex(int characterID) //gestisce indice e purchased
    {
        if (idPurchased.Contains(characterID))
        {
            lastIndex = idAssigned.Contains(characterID) ? 2 : 1;
            purchased = true;
        }
        else
        {
            lastIndex = 0;
            purchased = false;
        }
        SetHireButtonDefault();//setta il bottone di conseguenza
    }


    public void OnClickHIRE()
    {

            switch (lastIndex)
            {
                case 0:
                    if (doubloons >= cost && !purchased)
                    {
                        BuyPirate(characterID);
                        Image.sprite = sprites[1];
                        text.text = "Assign";
                        AudioManager.Instance.PlaySpecificOneShot(10);

                    }
                    else
                    {
                        anim.Play();
                        AudioManager.Instance.PlaySpecificOneShot(19);
                    }
                    break;
                case 1:
                    if (crewPanel.assignID.Count == 4)
                    {
                        AudioManager.Instance.PlaySpecificOneShot(19);
                    }
                    else
                    {
                        crewPanel.Assign(characterID,active.characterSprite,active.abilitySprite);
                        Image.sprite = sprites[2];
                        text.text = "Dismiss";
                        lastIndex = 2;
                        AddMainManuListIndex(characterID);
                        AudioManager.Instance.PlaySpecificOneShot(18);
                    }
                    break;

                case 2:
                    crewPanel.Dismiss(characterID);
                    Image.sprite = sprites[1];
                    text.text = "Assign";
                    lastIndex = 1;
                    RemoveMainManuListIndex(characterID);
                    AudioManager.Instance.PlaySpecificOneShot(17);
                   
                    break;

                default:
                    Image.sprite = sprites[0];
                    text.text = "Hire";
                    break;
            }
    }

    private void BuyPirate(int characterID)
    {
        costObj.SetActive(false);
        AddMainManuListPurchased(characterID);
        purchased = true;
        lastIndex = 1;
        doubloons -= cost;
        CrewController.Instance.doubloons = doubloons;
        doubloonsText.text = doubloons.ToString();
       
    }

    private void SetHireButtonDefault()
    {
        if (purchased == true)
        {
            costObj.SetActive(false);
            if (lastIndex == 1)
            {
                Image.sprite = sprites[1];
                text.text = "Assign";
            }
            else
            {
                Image.sprite = sprites[2];
                text.text = "Dismiss";
            }

        }
        else
        {
            costObj.SetActive(true);
            Image.sprite = sprites[0];
            text.text = "hire";
        }
    }

    public void AddMainManuListPurchased(int characterID)
    {
        if (!CrewController.Instance.idPurchased.Contains(characterID))
        {
            CrewController.Instance.idPurchased.Add(characterID);
        }
    }

    public void AddMainManuListIndex(int characterID)
    {
        if (!CrewController.Instance.idAssigned.Contains(characterID) && CrewController.Instance.idPurchased.Count < 5)
        {
            CrewController.Instance.idAssigned.Add(characterID);
        }
    }
    public void RemoveMainManuListIndex(int characterID)
    {
        if (CrewController.Instance.idAssigned.Contains(characterID))
        {
            CrewController.Instance.idAssigned.Remove(characterID);
        }
    }

}
