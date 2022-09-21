using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text pointsText;

    public void Start(){
        Setup();
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        pointsText.text = StaticVars.score.ToString() + " points";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Fight");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Splash");
    }
}
