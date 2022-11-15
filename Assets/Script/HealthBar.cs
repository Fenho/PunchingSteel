using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth (int health)
    {
        slider.maxValue = health;
        slider.value = health;
        StaticVars.score = 0;
        StaticVars.win = false;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        //StaticVars.addPoints( -health );
        if (health <= 0) {
            StaticVars.loseGame();
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
