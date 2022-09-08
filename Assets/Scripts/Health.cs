using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public static int playerHealth = 100;
    [SerializeField] public static int enemyHealth = 100;

    public static void TakeDamagePLayer(int damage)
    {
        playerHealth -= damage;
    }

    public static void TakeDamageEnemy(int damage) {
        Debug.Log("Enemy took " + damage + " damage! Health: " + enemyHealth);
        enemyHealth -= damage;
    }
}
