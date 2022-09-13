using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int teamHealth = 100;
    [SerializeField] private SimpleFlash flashEffect;
    [SerializeField] private EnemyHealthBar enemyHealthBar;
    [SerializeField] private HealthBar teamHealthBar;

    public void Start()
    {
        enemyHealthBar.SetMaxHealth(100);
        teamHealthBar.SetMaxHealth(100);
    }

    public void TakeDamageEnemy(int damage) 
    {
        flashEffect.Flash();
        enemyHealth -= damage;
        enemyHealthBar.SetHealth(enemyHealth);
    }

    public void TakeDamageTeam(int damage)
    {
        teamHealth -= damage;
        teamHealthBar.SetHealth(teamHealth);
    }
}
