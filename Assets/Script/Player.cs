using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    public float volume = 1.0f;

    [SerializeField] protected float jabTime = 0.5f;

    // Health and GameLogic
    protected int DAMAGE = 15;
    public GameLogic gameLogic;
    [SerializeField] public string teamState = RobotState.IDLE;

    // Animation Variables
    [SerializeField] public string playerState = RobotState.IDLE;

    // Stamina
    protected int maxStamina = 100;
    protected int currentStamina = 100;
    protected int DODGE_STAMINA_PENALTY = 12;
    protected int HIT_STAMINA_PENALTY = 20;
    [SerializeField] protected StaminaBar stamina;
    // Every second the player will win stamina
    protected float interval = 1.0f; 
    protected float nextTime = 0;

    public bool jabActivated = true;
    public bool dodgeActivated = true;
    public bool blockActivated = true;

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

    public override void OnJab(InputAction.CallbackContext context) {
        if (IsGameOver()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.JAB && stamina.slider.value > HIT_STAMINA_PENALTY && jabActivated) {
                playerState = teamState = RobotState.JAB;
                animator.Play(RobotState.JAB);
                StartCoroutine(LetAnimationRunForTime(jabTime));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE, "Right");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY);
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
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY)) {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                SendMessageToObserver("OnNotEnoughStamina");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY);
            }
        }
    }

    public override void OnRight(InputAction.CallbackContext context) {
        if (IsGameOver()) return;
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.RIGHT && stamina.slider.value > HIT_STAMINA_PENALTY) {
                playerState = teamState = RobotState.RIGHT;
                animator.Play(RobotState.RIGHT);
                StartCoroutine(LetAnimationRunForTime(jabTime));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE, "Left");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY);
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
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY))
            {
                flashEffect.Flash2();
                StaticVars.addPointsByType("UncoordinatedTeam");
                SendMessageToObserver("OnNotEnoughStamina");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY);
            }
        }
    }

    public override void OnDodgeRight(InputAction.CallbackContext context) {
        if (IsGameOver()) return;
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
        if (IsGameOver()) return;
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
        return;
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
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
        if (Time.time >= nextTime) {
            // Debug.Log("Time.time: " + Time.time);
            // Debug.Log("Interval: " + interval);
            // Debug.Log("NextTime: " + nextTime);
            stamina.IncreaseStaminaBy(10);
            nextTime += interval; 
            // Debug.Log("NextTime: " + nextTime);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsGameOver()) return;
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
    }
}
