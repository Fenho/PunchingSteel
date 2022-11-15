using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    private GameObject trainerGo;
    private Trainer trainer;

    // Music
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public AudioClip missSound;
    public AudioClip blockSound;

    public float volume = 1.0f;

    [SerializeField] private float jabSpeed = 0.5f;

    // Health and GameLogic
    private int DAMAGE = 15;
    public GameLogic gameLogic;
    [SerializeField] public string teamState = State.IDLE;

    public Animator animator;

    // Animation Variables
    [SerializeField] public string playerState = State.IDLE;

    // Stamina
    public int maxStamina = 100;
    public int currentStamina = 100;
    private int DODGE_STAMINA_PENALTY = 12;
    private int HIT_STAMINA_PENALTY = 20;
    [SerializeField] private StaminaBar stamina;
    // Every second the player will lose stamina
    int interval = 1; 
    float nextTime = 0;

    public void PlayBlockSound() {
        audioSource.PlayOneShot(blockSound, volume);
    }

    private SimpleFlash flashEffect;

    private void OnDisable() {
        jabAction.Disable();
        rightAction.Disable();
    }

    private void Awake() {
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
        // blockAction = playerInput.actions["Block"];
        // blockAction.performed += OnBlock;
        // blockAction.canceled += OnBlock;
        dodgeRightAction = playerInput.actions["DodgeRight"];
        dodgeRightAction.performed += OnDodgeRight;
        dodgeLeftAction = playerInput.actions["DodgeLeft"];
        dodgeLeftAction.performed += OnDodgeLeft;
    }

    // Start is called before the first frame update
    void Start()
    {
        stamina.SetMaxStamina(100);
    }

    private bool DoingSomething() {
        return !playerState.Equals(State.IDLE);
    }

    private bool isTeamBlocking() {
        if(teamState.Equals(State.BLOCK)){
            flashEffect.Flash();
            stamina.SetStamina(100);
            return true;
        }
        return false ;
    }

    private void OnJab(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.JAB && stamina.slider.value > HIT_STAMINA_PENALTY) {
                playerState = teamState = State.JAB;
                animator.Play(State.JAB);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE);
                stamina.SetStamina(HIT_STAMINA_PENALTY);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound1, volume);
                    StaticVars.addPoints(100);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPoints(-10);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPoints(-10);
                }
            }
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY)) {
                flashEffect.Flash2();
            }
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.RIGHT && stamina.slider.value > HIT_STAMINA_PENALTY) {
                playerState = teamState = State.RIGHT;
                animator.Play(State.RIGHT);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageEnemy(DAMAGE);
                stamina.SetStamina(HIT_STAMINA_PENALTY);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound2, volume);
                    StaticVars.addPoints(100);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPoints(-10);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPoints(-10);
                }
            }
            else if (stamina.slider.value <= (HIT_STAMINA_PENALTY + trainer.HIT_STAMINA_PENALTY))
            {
                flashEffect.Flash2();
            }
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        return;
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY) {
                playerState = teamState = State.DODGE_RIGHT;
                animator.Play(State.DODGE_RIGHT);
                stamina.SetStamina(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound1, volume);
                StaticVars.addPoints(50);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
            } else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                stamina.SetStamina(5);
            }
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (!isTeamBlocking()) {
            if (context.ReadValueAsButton() && !DoingSomething() && stamina.slider.value > DODGE_STAMINA_PENALTY) {
                playerState = teamState = State.DODGE_LEFT;
                animator.Play(State.DODGE_LEFT);
                stamina.SetStamina(DODGE_STAMINA_PENALTY);
                audioSource.PlayOneShot(dodgeSound2, volume);
                StaticVars.addPoints(50);
                StartCoroutine(LetAnimationRunForTime(jabSpeed));
            }
            else if (stamina.slider.value <= DODGE_STAMINA_PENALTY) {
                flashEffect.Flash2();
                stamina.SetStamina(5);
            }
        }
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

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime) {
            stamina.SetStamina(-10);
            nextTime += interval; 
        }
        if (trainer.trainerState == State.BLOCK) {
            playerState = teamState = State.BLOCK;
            animator.Play(State.BLOCK);
        }
        if (trainer.trainerState != State.BLOCK && !(DoingSomething() && !playerState.Equals(State.BLOCK))) {
            playerState = teamState = State.IDLE;
            animator.Play(State.IDLE);
        }
    }
}
