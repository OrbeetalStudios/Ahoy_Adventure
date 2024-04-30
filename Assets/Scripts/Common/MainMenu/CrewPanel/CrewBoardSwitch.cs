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
    private GameObject PanelSolo;
    [SerializeField]
    private GameObject PanelCrew;



    public void OnlightSolo()
    {
        soloImage.sprite = LightSolo;
        PanelCrew.SetActive(false);
        CrewImage.sprite = defCrew;
    }

    public void OnLightCrew()
    {
        soloImage.sprite = defSolo;
        PanelCrew.SetActive(true);
        CrewImage.sprite = LightCrew;
    }
}
  

