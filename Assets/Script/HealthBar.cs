using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private EndGameAnimations endGameAnimations;
    public float koAnimationDuration = 8.0f;
    // Ideally, the StaticVars should hold the reference to the GameSounds,
    // but as it is a static class we can't link it to the GameSounds instance.
    // The GameLogic is not called when the game is over, so we can't use it either.
    public GameSounds gameSounds;

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
            gameSounds.PlayEndGame();
            endGameAnimations.playKO();
            StartCoroutine(LoadGameOverAfterDelay(koAnimationDuration));
        }
    }

    IEnumerator LoadGameOverAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        SceneManager.LoadScene("GameOverScene");
    }
}
