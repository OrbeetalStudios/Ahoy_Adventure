using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlunderBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxPlunderTime(int time){
        slider.maxValue = time;
        slider.value = 0;
    }

    public void SetPlunderTime(int time){
        slider.value = time;
    }
}
