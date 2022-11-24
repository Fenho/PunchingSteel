using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameAnimations : MonoBehaviour
{
    public GameObject koTextRects;
    public Animation textRectsAnimation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
    }

    public void playKO()
    {
        textRectsAnimation.Play("KO");
        koTextRects.SetActive(true);
    }

    public void removeKO()
    {
        koTextRects.SetActive(false);
    }
}