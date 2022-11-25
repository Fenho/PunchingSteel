using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxStamina (int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void DecreaseStaminaBy(int stamina)
    {
        if(slider.value - stamina >= slider.maxValue){
            slider.value = slider.maxValue;
        }
        else if (slider.value - stamina <= 0){
            slider.value = 0;
        }
        else{
            slider.value -= stamina;
        }

    }

    public void IncreaseStaminaBy(int stamina)
    {
        if(slider.value + stamina >= slider.maxValue){
            slider.value += slider.maxValue;
        }
        else {
            slider.value += stamina;
        }
    }
}
