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
    [SerializeField] private float blockDamageFactor = 0.03f;

    public enum PunchResult {
        MISS,
        BLOCK,
        HIT,
    }

    public void Start()
    {
        enemyHealthBar.SetMaxHealth(100);
        teamHealthBar.SetMaxHealth(100);
    }

    // Returns true if the enemy took damage
    public PunchResult TakeDamageEnemy(int damage) 
    {
        PunchResult action = PunchResult.MISS;

        int randomValue = Random.Range(0, 99);

        if (enemy.enemyState == State.IDLE && randomValue < 50) {
            enemy.SetBlocking();
        }

        if (enemy.enemyState == State.DODGE_LEFT || enemy.enemyState == State.DODGE_RIGHT) {
            return action;
        }

        if (enemy.enemyState == State.BLOCK) {
            action = PunchResult.BLOCK;
            damage = (int) (damage * blockDamageFactor);
        } else {
            action = PunchResult.HIT;
        }

        flashEffect.Flash();
        enemyHealth -= damage;
        enemyHealthBar.SetHealth(enemyHealth);
        return action;
    }

    // Returns true if team was damaged
    public PunchResult TakeDamageTeam(int damage)
    {
        PunchResult action = PunchResult.MISS;

        if (robot.teamState == State.DODGE_LEFT || robot.teamState == State.DODGE_RIGHT) {
            return action;
        }

        if (robot.teamState == State.BLOCK) {
            action = PunchResult.BLOCK;
            damage = (int) (damage * blockDamageFactor);
        } else {
            action = PunchResult.HIT;
        }

        teamHealth -= damage;
        teamHealthBar.SetHealth(teamHealth);
        return action;
    }
}
