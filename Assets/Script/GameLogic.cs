using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static int MAX_HEALTH = 100;
    private int enemyHealth = MAX_HEALTH;
    private int teamHealth = MAX_HEALTH;
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
        enemyHealthBar.SetMaxHealth(enemyHealth);
        teamHealthBar.SetMaxHealth(teamHealth);
        StaticVars.setGameOver(false);
    }

    // Returns true if the enemy took damage
    public PunchResult TakeDamageEnemy(int damage, string side) 
    {
        PunchResult action = PunchResult.MISS;

        if (enemy.enemyState == RobotState.DODGE_LEFT || enemy.enemyState == RobotState.DODGE_RIGHT) {
            return action;
        }
        enemy.TakeDamage(side); // This is just for animation purposes
        enemy.ReactToHit(); // Useful to determine if the enemy is going to block or not (useful for having enemies with different behaviours)
        if (enemy.enemyState == RobotState.BLOCK) {
            action = PunchResult.BLOCK;
            damage = (int) (damage * blockDamageFactor);
        } else {
            action = PunchResult.HIT;
        }
        if (enemy.GetEnemyType() == "TrainingBag") {
            DamageEnemy(damage, 20);
        }
        else {
            DamageEnemy(damage);
        }
        return action;
    }

    public PunchResult CheckRobotDodge()
    {
        switch(enemy.GetEnemyType())
        {
            case "Enemy":
                if (enemy.enemyState == EnemyState.RIGHT && robot.teamState == RobotState.DODGE_LEFT) {
                    return PunchResult.MISS;
                }
                if (enemy.enemyState == EnemyState.JAB && robot.teamState == RobotState.DODGE_RIGHT) {
                    return PunchResult.MISS;
                }
                break;
            case "HeadBoxEnemy":
                if (enemy.enemyState == EnemyState.RIGHT && !(robot.teamState == RobotState.DODGE_LEFT)) {
                    return PunchResult.MISS;
                }
                if (enemy.enemyState == EnemyState.JAB && !(robot.teamState == RobotState.DODGE_RIGHT)) {
                    return PunchResult.MISS;
                }
                if (enemy.enemyState == EnemyState.HEAD_BUTT && (robot.teamState == RobotState.DODGE_LEFT || robot.teamState == RobotState.DODGE_RIGHT)) {
                    return PunchResult.MISS;
                }
                if (enemy.enemyState == EnemyState.DOUBLE && !(robot.teamState == RobotState.DODGE_LEFT || robot.teamState == RobotState.DODGE_RIGHT)) {
                    return PunchResult.MISS;
                }
                // Triple is undodgable
                break;
        }
        return PunchResult.HIT;
    }

    // Returns true if team was damaged
    public PunchResult TakeDamageTeam(int damage)
    {
        PunchResult action = CheckRobotDodge();
        if (action == PunchResult.MISS) return action;
        if (robot.teamState == RobotState.BLOCK) {
            action = PunchResult.BLOCK;
            damage = (int) (damage * blockDamageFactor);
        } else {
            action = PunchResult.HIT;
        }
        DamageTeam(damage);
        return action;
    }

    public void DamageEnemy(int amount, int healthFloor = 0) {
        if (enemyHealth - amount <= healthFloor && healthFloor != 0) {
            enemyHealth = MAX_HEALTH;
        } else {
            enemyHealth -= amount;
        }
        enemyHealth -= amount;
        enemyHealthBar.SetHealth(enemyHealth);
    }

    public void DamageTeam(int amount) {
        teamHealth -= amount;
        teamHealthBar.SetHealth(teamHealth);
    }

    public void HealEnemy(int amount) {
        enemyHealth += amount;
        enemyHealthBar.SetHealth(enemyHealth);
    }

    public void HealTeam(int amount) {
        teamHealth += amount;
        teamHealthBar.SetHealth(teamHealth);
    }
}
