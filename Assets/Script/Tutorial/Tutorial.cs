using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI trainerText;
    [SerializeField] private TextMeshProUGUI robotText;
    [SerializeField] private TextMeshProUGUI extraText;

    [SerializeField] private Button playButton;
    public static int n_punches_right = 0;
    public static int n_punches_left = 0;
    public static int n_dodges = 0;
    public static int n_blocks = 0;

    private GameObject robotGo;

    public bool jabActivated = false;
    public bool dodgeActivated = false;
    public bool blockActivated = false;

    public bool IsJabActivated()
    {
        return jabActivated;
    }
    public bool IsDodgeActivated()
    {
        return dodgeActivated;
    }
    public bool IsBlockActivated()
    {
        return blockActivated;
    }

    IEnumerator initialWaiter()
    {

        //Wait for 4 seconds
        yield return new WaitForSeconds(3);
        trainerText.text = "Trainer: Press O";
        robotText.text = "Robot: Press E";
        feedbackText.text = "Try the Right Jab!";
        
    }

    private void Awake() {
        StartCoroutine(initialWaiter());
        robotGo = GameObject.Find("Robot");
    }

    private void Start() {
        // robot.DisableJab();
        // robot.DisableDodge();
        // robot.DisableBlock();
        robotGo.SendMessage("DisableJab");
        robotGo.SendMessage("DisableDodge");
        robotGo.SendMessage("DisableBlock");
    }

    private void OnJab()
    {
        if (n_punches_left < 3){
            n_punches_right += 1;
            n_punches_left += 1;
            feedbackText.text = "That's it! Again! "+ n_punches_left;
        }
        
        
        if (n_punches_left == 3){
            dodgeActivated = true;
            robotGo.SendMessage("ActivateDodge");
            feedbackText.text = "Let's try Dodging";
            trainerText.text = "Trainer: Press J or L";
            robotText.text = "Robot: Press A or D";
        }
    }

    private void OnRight()
    {
        if (n_punches_right < 3){
                n_punches_right += 1;
                feedbackText.text = "Great! Again! " + n_punches_right;
            }
            
            if (n_punches_right == 3){
                jabActivated = true;
                // robotGo.ActivateJab();
                robotGo.SendMessage("ActivateJab");
                feedbackText.text = "Now try a Left Jab!";
                trainerText.text = "Trainer: Press U";
                robotText.text = "Robot: Press Q";
            }
    }

    private void OnRobotBlock()
    {
        extraText.text = "The robot has no say on blocking";
    }

    private void OnDodge()
    {
        if (n_dodges < 3){
            n_punches_left += 1;
            n_dodges += 1;
            feedbackText.text = "Great dodge! Again! "+ n_dodges;
        }
        
        
        if (n_dodges == 3){
            blockActivated = true;
            robotGo.SendMessage("ActivateBlock");
            feedbackText.text = "Finally, try blocking!";
        }
    }

    private void OnDodgePenalty()
    {
        extraText.text = "Blocking and dodging penalizes your stamina!";
    }

    private void OnBlock()
    {
        n_blocks += 1;
        if (n_blocks >= 3){
            feedbackText.text = "You make a Punching Team!";
            robotText.text = "Wanna play?";
            trainerText.text = "Let's do it!";
            playButton.gameObject.SetActive(true);
        }
    }

    private void OnNotEnoughStamina()
    {
        extraText.text = "That blue bar is your stamina, you need more to move. But you can still block while you wait for it to fill up!";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
