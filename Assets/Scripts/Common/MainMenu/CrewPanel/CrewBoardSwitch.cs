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
    [SerializeField]
    private GameObject pageDescription;
    [SerializeField]
    private GameObject pageCrew;



    public void OnlightSolo()
    {
        soloImage.sprite = LightSolo;
        //PanelDescription.SetActive(true);
        pageScrollOut(pageCrew);
        //pageScrollIn(pageDescription);
        CrewImage.sprite = defCrew;
    }

    public void OnLightCrew()
    {
        soloImage.sprite = defSolo;
        //PanelDescription.SetActive(false);
        //pageScrollOut(pageDescription);
        pageScrollIn(pageCrew);
        CrewImage.sprite = LightCrew;
    }

    public void pageScrollIn(GameObject panel)
    {
        LeanTween.moveLocalX(panel, 1f, 0.2f).setEase(LeanTweenType.easeInCubic);
    }

    public void pageScrollOut(GameObject panel)
    {
        LeanTween.moveLocalX(panel, 1000f, 0.2f).setEase(LeanTweenType.easeOutCubic);
    }
}
  

