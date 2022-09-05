using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " points";

    }

    public void RestartButton()
    {
        //SceneManager.LoadScene("Game");
    }

    public void MainMenuButton()
    {
        //SceneManager.LoadScene("Main Menu");
    }
}
