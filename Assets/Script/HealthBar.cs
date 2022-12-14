using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Healthbar logic
public class HealthBar : MonoBehaviour
{
    // Variables
    public float health;
    private float lerpTimer;
    public float maxHealth;
    public float chipSpeed;

    // Images
    public Image frontHealthBar;
    public Image backHealthBar; 

    [SerializeField] private EndGameAnimations endGameAnimations;
    public float koAnimationDuration = 8.0f;
    // Ideally, the StaticVars should hold the reference to the GameSounds,
    // but as it is a static class we can't link it to the GameSounds instance.
    // The GameLogic is not called when the game is over, so we can't use it either.
    public GameSounds gameSounds;

    void Start()
    {
        // Set health to max health
        health = maxHealth;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        // Set health bar to health
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth; // value between 0 and 1
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        // Take damage
        health -= damage;
        lerpTimer = 0f;
        // Update health bar
        UpdateHealthUI();
    }

    public void HealDamage(float heal)
    {
        // Heal damage
        health += heal;
        // Update health bar
        UpdateHealthUI();
    }

    public void SetMaxHealth (int value)
    {
        maxHealth = value;
        health = value;
        StaticVars.score = 0;
        StaticVars.win = false;
    }

    public void SetHealth(int newHealth)
    {
        // Take damage
        TakeDamage(health - newHealth);
        // Update health bar
        UpdateHealthUI();
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
        
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Fight") 
        {
            SceneManager.LoadScene("GameOverScene");
        } 
        else
        {
            SceneManager.LoadScene("GameOverSceneEnemy2");
        }

    }
}
