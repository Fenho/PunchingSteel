using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class Player : AbstractRobot
{
    // Observer
    [SerializeField] private GameObject observer;

    // Input management
    [SerializeField] protected PlayerInput playerInput;
    protected InputAction jabAction;
    protected InputAction rightAction;
    protected InputAction blockAction;
    protected InputAction dodgeLeftAction;
    protected InputAction dodgeRightAction;

    public Animator animator;
    [SerializeField] protected SimpleFlash flashEffect;
    [SerializeField] protected GameObject trainerGo;
    [SerializeField] protected Trainer trainer;

    // Sounds
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public AudioClip missSound;
    public AudioClip blockSound;
    // Combo sounds
    public AudioSource comboAudioSource;
    public AudioClip comboSound;
    public float comboVolume = 0.15f;

    public float volume = 1.0f;

    [SerializeField] protected float jabTime = 0.5f;

    // Health and GameLogic
    protected int JAB_DAMAGE = 1;
    protected int RIGHT_DAMAGE = 3;
    public GameLogic gameLogic;
    [SerializeField] public string teamState = RobotState.IDLE;

    // Animation Variables
    [SerializeField] public string playerState = RobotState.IDLE;
    protected bool shouldPlayWinLoseAfterGameOver = true;

    // Stamina
    protected int STAMINA_REGEN = 8;
    protected int maxStamina = 100;
    protected int currentStamina = 100;
    protected int DODGE_STAMINA_PENALTY = 8;
    protected int JAB_STAMINA_PENALTY = 3;
    protected int RIGHT_STAMINA_PENALTY = 9;
    [SerializeField] protected StaminaBar stamina;

    public bool jabActivated = true;
    public bool dodgeActivated = true;
    public bool blockActivated = true;

    // Combo state multiplier and counters
    private double comboMultiplier = 1.5;
    [SerializeField] private int comboCounter = 0;
    private int comboMax = 3;
    // TTL for combo
    private float comboExpirationTime = 2.0f;
    [SerializeField] private float comboExpirationTimeLeft = 0.0f;

    // Combo UI
    public FightPoints fightPoints;


    public void Awake() {
        flashEffect = GetComponent<SimpleFlash>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        trainerGo = GameObject.Find("Trainer");
        if (trainerGo != null) {
            trainer = trainerGo.GetComponent<Trainer>();
        }
        jabAction = playerInput.actions["Jab"];
        jabAction.performed += OnJab;
        rightAction = playerInput.actions["RightDirect"];
        rightAction.performed += OnRight;
        dodgeRightAction = playerInput.actions["DodgeRight"];
        dodgeRightAction.performed += OnDodgeRight;
        dodgeLeftAction = playerInput.actions["DodgeLeft"];
        dodgeLeftAction.performed += OnDodgeLeft;
        blockAction = playerInput.actions["Block"];
        blockAction.performed += OnBlock;
        observer = GameObject.FindWithTag("Observer");
    }

    // Start is called before the first frame update
    void Start()
    {
        stamina.SetMaxStamina(100);
    }
    
    public void PlayBlockSound() {
        audioSource.PlayOneShot(blockSound, volume);
    }

    private void OnDisable() {
        jabAction.Disable();
        rightAction.Disable();
    }

    private bool DoingSomething() {
        return !playerState.Equals(RobotState.IDLE);
    }

    private bool IsGameOver() {
        return StaticVars.gameOver;
    }

    private bool isTeamBlocking() {
        if(teamState.Equals(RobotState.BLOCK)){
            flashEffect.Flash();
            stamina.DecreaseStaminaBy(100);
            return true;
        }
        return false ;
    }

    private void SendMessageToObserver(string message) {
        if (observer != null) {
            observer.SendMessage(message);
        }
    }

    public void ActivateJab()
    {
        jabActivated = true;
    }

    public void DisableJab()
    {
        jabActivated = false;
    }

    public void ActivateDodge()
    {
        dodgeActivated = true;
    }

    public void DisableDodge()
    {
        dodgeActivated = false;
    }

    public void ActivateBlock()
    {
        blockActivated = true;
    }

    public void DisableBlock()
    {
        blockActivated = false;
    }

    private bool isInPauseMenu()
    {
        return StaticVars.isInPauseMenu;
    }

    private void HandleCombo(GameLogic.PunchResult punchResult) {
        if (!punchResult.Equals(GameLogic.PunchResult.HIT)) {
            ResetCombo();
            return;
        } else {
            comboCounter = Math.Min(comboCounter + 1, comboMax);
            comboExpirationTimeLeft = comboExpirationTime;
            if (comboAudioSource != null && comboSound != null) {
                comboAudioSource.pitch = (float)Math.Pow(comboMultiplier, comboCounter);
                comboAudioSource.PlayOneShot(comboSound, comboVolume);
            }
            if (fightPoints != null)
                fightPoints.doComboEffect(comboCounter);
        }
    }

    private void UpdateComboExpiration() {
        if (comboExpirationTimeLeft > 0.0f) {
            comboExpirationTimeLeft -= Time.deltaTime;
            if (comboExpirationTimeLeft <= 0.0f) {
                ResetCombo();
            }
        }
    }

    public void ResetCombo() {
        comboCounter = 0;
        comboExpirationTimeLeft = 0.0f;
        if (comboAudioSource != null)
            comboAudioSource.pitch = 1.0f;
    }


    public override void OnJab(InputAction.CallbackContext context) {
        if (IsGameOver() || isInPauseMenu()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.JAB && stamina.slider.value > JAB_STAMINA_PENALTY && jabActivated) {
                playerState = teamState = RobotState.JAB;
                animator.Play(RobotState.JAB);
                StartCoroutine(LetAnimationRunForTime(jabTime));
                int damageWithCombo = (int)Math.Ceiling(JAB_DAMAGE * Math.Pow(comboMultiplier, comboCounter));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(damageWithCombo, "Right");
                stamina.DecreaseStaminaBy(JAB_STAMINA_PENALTY);
                HandleCombo(punchResult);
                
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound1, volume);
                    StaticVars.addPointsByType("LeftJabTeam");
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPointsByType("LeftJabMissTeam");
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPointsByType("LeftJabBlockTeam");
                }
                SendMessageToObserver("OnJab");
            }
            else if (stamina.slider.value <= (JAB_STAMINA_PENALTY + trainer.JAB_STAMINA_PENALTY)) {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                SendMessageToObserver("OnNotEnoughStamina");
                stamina.DecreaseStaminaBy(JAB_STAMINA_PENALTY + trainer.JAB_STAMINA_PENALTY);
            }
        }
    }

    public override void OnRight(InputAction.CallbackContext context) {
        if (IsGameOver() || isInPauseMenu()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.RIGHT && stamina.slider.value > RIGHT_STAMINA_PENALTY) {
                playerState = teamState = RobotState.RIGHT;
                animator.Play(RobotState.RIGHT);
                StartCoroutine(LetAnimationRunForTime(jabTime));
                int damageWithCombo = (int)Math.Ceiling(RIGHT_DAMAGE * Math.Pow(comboMultiplier, comboCounter));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(damageWithCombo, "Left");
                stamina.DecreaseStaminaBy(RIGHT_STAMINA_PENALTY);
                HandleCombo(punchResult);

                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound2, volume);
                    StaticVars.addPointsByType("RightJabTeam");
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPointsByType("RightJabMissTeam");
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPointsByType("RightJabBlockTeam");
                }
                SendMessageToObserver("OnRight");
            }
            else if (stamina.slider.value <= (RIGHT_STAMINA_PENALTY + trainer.RIGHT_STAMINA_PENALTY))
            {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                SendMessageToObserver("OnNotEnoughStamina");
                stamina.DecreaseStaminaBy(RIGHT_STAMINA_PENALTY + trainer.RIGHT_STAMINA_PENALTY);
            }
        }
    }

    public override void OnDodgeRight(InputAction.CallbackContext context) {
        if (IsGameOver() || isInPauseMenu()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY && dodgeActivated) {
                playerState = teamState = RobotState.DODGE_RIGHT;
                animator.Play(RobotState.DODGE_RIGHT);
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound1, volume);
                StaticVars.addPointsByType("RightDodgeTeam");
                StartCoroutine(LetAnimationRunForTime(jabTime));
                SendMessageToObserver("OnDodge");
            } else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                SendMessageToObserver("OnDodgePenalty");
            }
        }
    }

    public override void OnDodgeLeft(InputAction.CallbackContext context) {
        if (IsGameOver() || isInPauseMenu()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY && dodgeActivated) {
                playerState = teamState = RobotState.DODGE_LEFT;
                animator.Play(RobotState.DODGE_LEFT);
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound2, volume);
                StaticVars.addPointsByType("LeftDodgeTeam");
                StartCoroutine(LetAnimationRunForTime(jabTime));
                SendMessageToObserver("OnDodge");
            }
            else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                SendMessageToObserver("OnDodgePenalty");
            }
        }
    }

    public void OnBlock(InputAction.CallbackContext context) {
        if (IsGameOver()) return;
        SendMessageToObserver("OnRobotBlock");
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    protected IEnumerator PlayWinLoseAfterGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        // The Static Vars win variable is set to true if the player has won
        if (StaticVars.win) {
            animator.Play(RobotState.WIN);
        } else {
            animator.Play(RobotState.LOSE);
        }
    }

    private void BackToIdle() {
        if (playerState == RobotState.BLOCK) {
            animator.Play(RobotState.BLOCK);
        } else {
            animator.Play(RobotState.IDLE);
        }
        playerState = teamState = RobotState.IDLE;
    }

    void Regen() {
        stamina.IncreaseStaminaBy(STAMINA_REGEN * Time.deltaTime);
    }

    void UpdateCombo() {
        if (comboExpirationTimeLeft - Time.deltaTime <= 0) {
            comboExpirationTimeLeft = 0;
            comboCounter = 0;
        } else {
            comboExpirationTimeLeft -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (IsGameOver()) {
            if (shouldPlayWinLoseAfterGameOver) {
                StartCoroutine(PlayWinLoseAfterGameOver(jabTime));
                shouldPlayWinLoseAfterGameOver = false;
            }
            return;
        }
        
        Regen();
        if (trainer.trainerState == RobotState.BLOCK && blockActivated) {
            playerState = teamState = RobotState.BLOCK;
            animator.Play(RobotState.BLOCK);
            SendMessageToObserver("OnBlock");
        }
        if (trainer.trainerState != RobotState.BLOCK && !(DoingSomething() && !playerState.Equals(RobotState.BLOCK))) {
            playerState = teamState = RobotState.IDLE;
            animator.Play(RobotState.IDLE);
        }

        // Reduce the time left for the combo
        UpdateComboExpiration();
    }
}
