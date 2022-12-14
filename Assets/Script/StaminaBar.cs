using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public NoStaminaScript noStaminaScript;
    public Slider slider;

    public void SetMaxStamina (int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void DecreaseStaminaBy(float stamina)
    {
        if(slider.value - stamina >= slider.maxValue){
            slider.value = slider.maxValue;
        }
        else if(slider.value - stamina <= 0){
            slider.value = 0;
            noStaminaScript.ShowText();
        }
        else{
            slider.value -= stamina;
        }
    }

    public void IncreaseStaminaBy(float stamina)
    {
        if(slider.value + stamina >= slider.maxValue){
            slider.value += slider.maxValue;
        }
        else {
            slider.value += stamina;
        }
    }
}
