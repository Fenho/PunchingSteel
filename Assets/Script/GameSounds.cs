using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour 
{
    public AudioSource endGameSource;
    public AudioSource musicSource;
    public AudioSource crowdBackgroundSource;
    public AudioClip   endGameClip;
    public float fadeOutDuration = 4.0f;

    public void PlayEndGame()
    {
        endGameSource.PlayOneShot(endGameClip);
        StartCoroutine(FadeAudioSource.StartFade(musicSource, fadeOutDuration, 0));
        StartCoroutine(FadeAudioSource.StartFade(crowdBackgroundSource, fadeOutDuration, 0));
    }
}
