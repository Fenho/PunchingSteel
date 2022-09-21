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
    [SerializeField] private Enemy enemy;
    [SerializeField] private Player robot;
    [SerializeField] private float blockDamageFactor = 0.5f;

    public void Start()
    {
        enemyHealthBar.SetMaxHealth(100);
        teamHealthBar.SetMaxHealth(100);
    }

    public bool TakeDamageEnemy(int damage) 
    {
        if (robot.teamState == State.DODGE_LEFT || robot.teamState == State.DODGE_RIGHT) {
            return false;
        }

        if (robot.teamState == State.BLOCK) {
            damage = (int) (damage * blockDamageFactor);
        }

        flashEffect.Flash();
        enemyHealth -= damage;
        enemyHealthBar.SetHealth(enemyHealth);
        return true;
    }

    // Returns true if team was damaged
    public bool TakeDamageTeam(int damage)
    {
        if (enemy.enemyState == State.DODGE_LEFT || enemy.enemyState == State.DODGE_RIGHT) {
            return false;
        }

        if (enemy.enemyState == State.BLOCK) {
            damage = (int) (damage * blockDamageFactor);
        }

        teamHealth -= damage;
        teamHealthBar.SetHealth(teamHealth);
        return true;
    }
}
