using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }

    public bool enemyUnlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            enemyUnlocked = false;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockEnemy(bool unlocked){
        enemyUnlocked = unlocked;
    }
}
