using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class CrewBoardSwitch : MonoBehaviour
{
    [SerializeField]
    private Image soloImage;
    [SerializeField]
    private Image CrewImage;
    [SerializeField]
    private Sprite LightSolo;
    [SerializeField]
    private Sprite LightCrew;
    [SerializeField]
    private Sprite defSolo;
    [SerializeField]
    private Sprite defCrew;
    [SerializeField]
    private GameObject PanelDescription;



    public void OnlightSolo()
    {
        soloImage.sprite = LightSolo;
        PanelDescription.SetActive(true);
        CrewImage.sprite = defCrew;
    }

    public void OnLightCrew()
    {
        soloImage.sprite = defSolo;
        PanelDescription.SetActive(false);
        CrewImage.sprite = LightCrew;
    }
}
  

