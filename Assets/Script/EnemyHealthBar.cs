using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private EndGameAnimations endGameAnimations;
    public float koAnimationDuration = 8.0f;
    public GameSounds gameSounds;

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
            PersistentManager.Instance.UnlockEnemy(true);
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
