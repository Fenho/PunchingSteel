using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnlocked : MonoBehaviour
{
    [SerializeField] private Button firstEnemy;
    [SerializeField] private Button secondEnemy;

    private void Awake()
    {
        if (PersistentManager.Instance.enemyUnlocked)
        {
            secondEnemy.gameObject.SetActive(true);
        }
    }

    public void UnlockSecondEnemy()
    {
        PersistentManager.Instance.UnlockEnemy(true);
        if (PersistentManager.Instance.enemyUnlocked)
        {
            secondEnemy.gameObject.SetActive(true);
        }
    }
}
