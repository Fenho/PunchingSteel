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
    public Text comboTextGO;
    
/*     public List<Text> effectList;

    private bool showScoreEffect1 = false;
    private bool showScoreEffect2 = false;
    private bool showScoreEffect3 = false;

    private int lastScore; */
    private int lastScore;
    private bool notDoingEffect;

    public void Start(){
        pointsText.text =  "0 points";
        lastScore = 0;
        notDoingEffect = true;
    }

    public void Update(){
        if(StaticVars.score != lastScore && notDoingEffect){
            doScoreEffect();
            lastScore = StaticVars.score;
        };
        pointsText.text = StaticVars.score.ToString() + " points!";
    }

    public void doScoreEffect(){
        StartCoroutine(textEffectWaiter());
    }

    IEnumerator textEffectWaiter()
    {
        notDoingEffect = false;
        if (StaticVars.score - lastScore > 0){
            scoreEffect1.gameObject.SetActive(true);
            scoreEffect1.text = "+" + (StaticVars.score - lastScore).ToString();
            yield return new WaitForSeconds(0.3f); 
            scoreEffect1.gameObject.SetActive(false);

        }else{
            scoreEffect2.gameObject.SetActive(true);
            scoreEffect2.text = (StaticVars.score - lastScore).ToString();
            yield return new WaitForSeconds(0.3f); 
            scoreEffect2.gameObject.SetActive(false);
        }
        notDoingEffect = true;
    }

    public void doComboEffect(int comboCounter){
        StartCoroutine(comboEffectWaiter(comboCounter));
    }

    IEnumerator comboEffectWaiter(int comboCounter)
    {
        comboTextGO.gameObject.SetActive(true);
        comboTextGO.text = "Combo x" + comboCounter.ToString();
        yield return new WaitForSeconds(0.3f); 
        comboTextGO.gameObject.SetActive(false);
    }

}
