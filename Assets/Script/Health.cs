using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private SimpleFlash flashEffect;
    [SerializeField] private EnemyHealthBar enemyHealthBar;

    public void Start()
    {
        enemyHealthBar.SetMaxHealth(100);
    }

    public void TakeDamageEnemy(int damage) 
    {
        flashEffect.Flash();
        enemyHealth -= damage;
        enemyHealthBar.SetHealth(enemyHealth);
        Debug.Log("Enemy took damage " + damage + "Health is now " + enemyHealth);
    }
}
