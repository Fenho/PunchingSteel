using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    public int playerHealth;
    public int enemyHealth;
    public HealthBar playerHealthBar;
    public EnemyHealthBar enemyHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthBar.SetHealth(playerHealth);
        enemyHealthBar.SetHealth(enemyHealth);
    }
}
