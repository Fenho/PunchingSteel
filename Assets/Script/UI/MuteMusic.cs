using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusic : MonoBehaviour
{
    [SerializeField] Toggle mutedToggle;

    private void Awake() {
        if (StaticVars.isMuted){
            mutedToggle.isOn = true;
        }else{
            mutedToggle.isOn = false;
        }
    }

    public void Mute(bool notMuted){
        if (StaticVars.isMuted){
            mutedToggle.isOn = false;
            AudioListener.volume = 1;
            StaticVars.isMuted = false;
        }else{
            mutedToggle.isOn = true;
            AudioListener.volume = 0;
            StaticVars.isMuted = true;
        }
    }
}
