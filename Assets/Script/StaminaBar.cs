using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public NoStaminaScript noStaminaScript;
    public Slider slider;
    public float stamina;
    public float maxStamina;
    private float lerpTimer;
    public float chipSpeed;
    
    void Start()
    {
        stamina = maxStamina;
        slider.value = maxStamina;
        slider.maxValue = maxStamina;
    }

    void Update()
    {
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        UpdateStaminahUI();
    }

    public void UpdateStaminahUI()
    {
        // Set health bar to health
        if(stamina != slider.value)
        {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            slider.value = Mathf.Lerp(stamina, slider.value, percentComplete);
        }
    }

    public void DecreaseStaminaBy(int energy)
    {
        lerpTimer = 0f;
        if(stamina - energy >= maxStamina){
            stamina = maxStamina;
        }
        else if(slider.value - energy <= 0){
            stamina = 0;
            noStaminaScript.ShowText();
        }
        else{
            stamina -= energy;
        }
        UpdateStaminahUI();
    }

    public void IncreaseStaminaBy(int energy)
    {
        if(stamina + energy >= maxStamina){
            stamina = maxStamina;
        }
        else {
            stamina += energy;
        }
        UpdateStaminahUI();
    }
}
