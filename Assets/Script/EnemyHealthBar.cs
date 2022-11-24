using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private EndGameAnimations endGameAnimations;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        //StaticVars.addPoints( health );
        if (health <= 0) {
            StaticVars.winGame();
            endGameAnimations.playKO();
            StartCoroutine(LetAnimationRunForTime(2f));
        }
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        SceneManager.LoadScene("GameOverScene");
    }
}
