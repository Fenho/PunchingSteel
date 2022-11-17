using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightPoints : MonoBehaviour
{

    public Text pointsText;
    public Text scoreEffect1;
    public Text scoreEffect2;
    public Text scoreEffect3;
    
/*     public List<Text> effectList;

    private bool showScoreEffect1 = false;
    private bool showScoreEffect2 = false;
    private bool showScoreEffect3 = false;

    private int lastScore; */

    public void Start(){
        pointsText.text =  "0 points";
        /* effectList = new List<Text>();
        effectList.Add(scoreEffect1);
        effectList.Add(scoreEffect2);
        effectList.Add(scoreEffect3); */
    }

    public void Update(){
        pointsText.text = StaticVars.score.ToString() + " points!";
        pointsText.text = StaticVars.score.ToString() + " points!";

    }

}
