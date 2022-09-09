using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMusicController : MonoBehaviour
{
    public static InitialMusicController instance; // Creates a static varible for a InitialMusicController instance

    private void Awake() // Runs before void Start()
    {
        DontDestroyOnLoad(this.gameObject); // Don't destroy this gameObject when loading different scenes

        if (instance == null) // If the InitialMusicController instance variable is null
        {
            instance = this; // Set this object as the instance
        }
        else // If there is already a InitialMusicController instance active
        {
            Destroy(gameObject); // Destroy this gameObject
        }
    }

    public void OnEnable()
    {
        Debug.Log("MusicController enabled");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void OnDisable()
    {
        Debug.Log("MusicController disabled");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        // here you can use scene.buildIndex or scene.name to check which scene was loaded
        if (scene.name == "Fight"){
            // Destroy the gameobject this script is attached to
            Destroy(gameObject);
        } 
    }
}