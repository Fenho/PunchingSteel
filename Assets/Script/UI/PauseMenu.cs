using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isInPause = false;

    public void Pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        StaticVars.enterPause();
    }

    public void Resume(){
        pauseMenu.SetActive(false);
        StaticVars.exitPause();
        Time.timeScale = 1f;
        isInPause = false;
    }

    public void Home(int sceneID){
        Time.timeScale = 1f;
        StaticVars.exitPause();
        isInPause = false;
        SceneManager.LoadScene(sceneID);
        
    }
}
