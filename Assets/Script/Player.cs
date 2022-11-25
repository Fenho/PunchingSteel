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

    [SerializeField] protected float jabSpeed = 0.5f;

    // Health and GameLogic
    protected int DAMAGE = 15;
    public GameLogic gameLogic;
    [SerializeField] public string teamState = State.IDLE;

    // Animation Variables
    [SerializeField] public string playerState = State.IDLE;

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
        return !playerState.Equals(State.IDLE);
    }

    private bool isTeamBlocking() {
        if(teamState.Equals(State.BLOCK)){
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
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.JAB && stamina.slider.value > HIT_STAMINA_PENALTY && jabActivated) {
                playerState = teamState = State.JAB;
                animator.Play(State.JAB);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE, "Right");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound1, volume);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                }
                SendMessageToObserver("OnJab");
            }
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY)) {
                flashEffect.Flash2();
                SendMessageToObserver("OnNotEnoughStamina");
            }
        }
    }

    public override void OnRight(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.RIGHT && stamina.slider.value > HIT_STAMINA_PENALTY) {
                playerState = teamState = State.RIGHT;
                animator.Play(State.RIGHT);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE, "Left");
                stamina.DecreaseStaminaBy(HIT_STAMINA_PENALTY);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound2, volume);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                }
                SendMessageToObserver("OnRight");
            }
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY))
            {
                flashEffect.Flash2();
                SendMessageToObserver("OnNotEnoughStamina");
            }
        }
    }

    public override void OnDodgeRight(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY && dodgeActivated) {
                playerState = teamState = State.DODGE_RIGHT;
                animator.Play(State.DODGE_RIGHT);
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound1, volume);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                SendMessageToObserver("OnDodge");
            } else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                SendMessageToObserver("OnDodgePenalty");
            }
        }
    }

    public override void OnDodgeLeft(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY && dodgeActivated) {
                playerState = teamState = State.DODGE_LEFT;
                animator.Play(State.DODGE_LEFT);
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound2, volume);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                SendMessageToObserver("OnDodge");
            }
            else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                stamina.DecreaseStaminaBy(DODGE_STAMINA_PENALTY);
                SendMessageToObserver("OnDodgePenalty");
            }
        }
    }

    public void OnBlock(InputAction.CallbackContext context) {
        SendMessageToObserver("OnRobotBlock");
        return;
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    private void BackToIdle() {
        if (playerState == State.BLOCK) {
            animator.Play(State.BLOCK);
        } else {
            animator.Play(State.IDLE);
        }
        playerState = teamState = State.IDLE;
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
        Regen();
        if (trainer.trainerState == State.BLOCK && blockActivated) {
            playerState = teamState = State.BLOCK;
            animator.Play(State.BLOCK);
            SendMessageToObserver("OnBlock");
        }
        if (trainer.trainerState != State.BLOCK && !(DoingSomething() && !playerState.Equals(State.BLOCK))) {
            playerState = teamState = State.IDLE;
            animator.Play(State.IDLE);
        }
    }
}
