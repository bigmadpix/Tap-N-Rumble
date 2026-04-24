using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using static BoxerAIEnemy;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class Player : MonoBehaviour
{
    public Transform eyeBox;
    private Rigidbody rb;
    public Vector3 standingPos = Vector3.zero;
    public float deltaX;
    private Vector2 startPosTouch;
    public float minSwipeDistance = 100f;
    // Movement speed in units per second.
    public float speed = 5F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public float dodgeRecovery = 1f;
    public float camSpeed = 1f;
    public Animator anim;
    public AnimatorOverrideController controller;

    public GameObject leftFist;
    public GameObject rightFist;

    // Sensitivity and timing settings (adjustable in Inspector)
    public float threshold = 2.0f;
    public float shakeCooldown = 0.5f;

    private Vector3 _lowPassValue;
    private float _timeSinceLastShake = 0;

    [SerializeField] private float playerStamina = 100;
    private float playerSP = 0;

    public static Player instance;
    public List<Perk> perks;

    public bool perfectDodge, dodge = false;
    public int dodgeTimer = 0;
    public GameObject enemyObj;
    public int combo;
    public int comboTimer;
    public float maxHP = 100;
    public int playerATK = 20;
    public float playerATKBuff = 0f;
    public bool isInvincible = false;
    public bool deathPrevention = false;
    public bool isDead = false;
    public float staminaConsumption = 0.1f;
    public bool isSouthpaw = false;
    public float guardDamage = 0.25f;
    //base global damage
    public float baseDMG = 1f;
    //vulnurability
    public float baseVULN = 1f;
    //Events for any script that refrences the player.
    public static event Action OnDodgeSuccess;
    public static event Action OnPerfectDodge;
    public static event Action OnComboIncrease;
    public static event Action OnPassive;
    public static event Action OnKnockout;
    public static event Action OnPunch;
    public static event Action OnPunchHit;
    //public static event Action OnHit;

    public enum MoveState
    {
        Neutral, 
        Dodge, 
        Punch
        
        
    }
    public MoveState moveState = MoveState.Neutral;
    private Vector3 startPos; Vector3 prev;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            
        }
    }
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown; 
        Touch.onFingerUp += OnFingerUp;
        playerStamina = 100f;
    }
    void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();

        if (Accelerometer.current != null)
        {
            InputSystem.DisableDevice(Accelerometer.current);
        }
    }
    void Start()
    {
        instance = this;
        if (GameManager.instance != null)
        {
            LoadData(GameManager.instance.playerDat);
        }
        rb = GetComponent<Rigidbody>();
        // Keep a note of the time the movement started.
        startTime = Time.time;
        controller = new AnimatorOverrideController(anim.runtimeAnimatorController);
        // Calculate the journey length.
        journeyLength = Vector3.Distance(eyeBox.position, standingPos * 5f);
        anim.runtimeAnimatorController = controller;
        if (Accelerometer.current != null)
        {
            // Start the Accelerometer
            _lowPassValue = Accelerometer.current.acceleration.ReadValue();
        }

        //Changes the color of the gloves on the player 
        leftFist.GetComponent<Renderer>().material.color = V.GloveColor;
        rightFist.GetComponent<Renderer>().material.color = V.GloveColor;


    }
    public void AddCombo()
    {
        combo++;
        comboTimer += 50;
        OnComboIncrease();
    }
    public void LoadData(PlayerData data)
    {
        maxHP = data.maxHP;
        playerStamina = data.currHP;
        perks = data.perks;
    }
    public void AddHP(float amount)
    {

        playerStamina += amount;


    }

    public float GetHP()
    {
        return playerStamina;
    }
    public float GetMaxHP()
    {
        return maxHP;
    }
    public float GetSP()
    {
        return playerSP;
    }

    public void AddSP(float sp)
    {
        playerSP += sp;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, eyeBox.position, Time.unscaledDeltaTime * camSpeed);
        Camera.main.transform.rotation = eyeBox.rotation;
    }
    void Update()
    {
        if (isDead)
        {
            anim.speed = 0;
        }
        enemyObj = GameObject.FindGameObjectWithTag("Enemy");

        if (moveState == MoveState.Dodge) {

            if(dodgeTimer <= 10 && enemyObj.GetComponent<BoxerAIEnemy>().ES == EnemyState.Punch && Vector3.Distance(transform.position, enemyObj.transform.position) <= 6f)
            {
                Debug.Log(Vector3.Distance(transform.position, enemyObj.transform.position));
                if (!perfectDodge && Vector3.Distance(transform.position, enemyObj.transform.position) <= 5f)
                {
                    perfectDodge = true;
                    OnPerfectDodge();
                }
                else if(!dodge)
                {
                    dodge = true;
                    OnDodgeSuccess();
                }

            }
            
                dodgeTimer++;

        }
        else
        {
            dodgeTimer = 0; 
            perfectDodge = false;
        }
        if (playerStamina <= 0)
        {
            Die();

        }
        _timeSinceLastShake += Time.unscaledDeltaTime;

        // Apply low-pass filter to remove gravity
        if (Accelerometer.current != null)
        {
            _lowPassValue = Vector3.Lerp(_lowPassValue, Accelerometer.current.acceleration.ReadValue(), 0.1f);
            Vector3 deltaAcceleration = Accelerometer.current.acceleration.ReadValue() - _lowPassValue;

            // Check for sharp movement exceeding the threshold
            if (deltaAcceleration.sqrMagnitude >= threshold && _timeSinceLastShake >= shakeCooldown)
            {
                Debug.Log("Shake detected!");
                // Trigger action (e.g., Handheld.Vibrate())
                _timeSinceLastShake = 0;
                OnShake();
            }
        }
        Vector3 deltaPos = transform.position - prev;
        deltaX = deltaPos.x;

        prev = transform.position;

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        Vector3 target = Vector3.Lerp(transform.position, (standingPos * 5f), Time.unscaledDeltaTime * speed);
        transform.position = new Vector3(target.x, transform.position.y, target.z);
        if (Keyboard.current[Key.A].wasPressedThisFrame) {
            StopAllCoroutines();
            StartCoroutine(DodgeLeft());
            return;

        }
        if (Keyboard.current[Key.D].wasPressedThisFrame)
        {
            StopAllCoroutines();
            StartCoroutine(DodgeRight());
            return;

        }
        if (Keyboard.current[Key.S].wasPressedThisFrame)
        {
            StopAllCoroutines();
            StartCoroutine(DodgeBack());
            return;

        }

        OnPassive();
        if (playerStamina > maxHP)
        {
            playerStamina = maxHP;
        }
        if (playerATKBuff < 0f)
        {
            playerATKBuff = 0f;
        }
    }
    public void OnPunchLand()
    {
        OnPunchHit();
    }
    public void PlayerGotPunched(float p)
    {
 
    }
    public void Die()
    {
        OnKnockout();
        if (!deathPrevention)
        {
            playerStamina = 0;
            isDead = true;
            UIManager.instance.GameOver();
        }
        else
        {
            playerStamina = 1;
            deathPrevention = false;
        }
    }
    public void OnHit(float p)
    {
        if (!isInvincible)
        {
            playerStamina -= p * baseVULN;
            UIManager.instance.HitVFX();
        }

    }
    public void OnShake()
    {
        if (isDead) { return; }
        Debug.Log("Initiating Super Special Move!!");
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null) { enemy.GetComponent<BoxerAIEnemy>().GetPunched(playerSP); playerSP = 0; } else
        {
            Debug.Log("Attack Missed. No Enemy Found");
        }
        
    }

    public void OnFingerDown(Finger finger)
    {
        if(!isDead)
            startPosTouch = finger.screenPosition;
        //Debug.Log(finger.screenPosition);
    }

    private void OnFingerUp(Finger finger)
    {
        if (isDead) {return; }
            Vector2 endPos = finger.screenPosition;
        Vector2 swipeDirection = endPos - startPosTouch;

        if (swipeDirection.magnitude >= minSwipeDistance)
        {
            // Determine the primary direction (horizontal or vertical)
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                Debug.Log(swipeDirection.x);
                // Horizontal swipe
                if (swipeDirection.x > 0)
                {
                    Debug.Log("Swiped Right");
                    StopAllCoroutines();
                    if (isSouthpaw)
                    {
                        StartCoroutine(DodgeLeft());
                    }
                    else
                    {
                        StartCoroutine(DodgeRight());
                    }
                }
                else
                {
                    Debug.Log("Swiped Left"); 
                    StopAllCoroutines();
                    if (isSouthpaw)
                    {
                        StartCoroutine(DodgeRight());
                    }
                    else
                    {
                        StartCoroutine(DodgeLeft());
                    }

                    
                }
            }
            else
            {
                // Vertical swipe
                if (swipeDirection.y > 0)
                {
                    Debug.Log("Swiped Up"); 
                    if (isSouthpaw)
                    {
                        Debug.Log("Swiped Up");
                        StopAllCoroutines();
                        StartCoroutine(DodgeBack());
                    }
                }
                else
                {
                    if (!isSouthpaw)
                    {
                        Debug.Log("Swiped Down");
                        StopAllCoroutines();
                        StartCoroutine(DodgeBack());
                    }

                }
            }
        }
        else
        {
            Debug.Log("Tapped."); 
            if (finger.screenPosition.x > Screen.width/2)
            {
                rightFist.GetComponent<PlayerFist>().isHitboxActive = true;
                Debug.Log("Punch Right");
                StopAllCoroutines();
                StartCoroutine(Punch());
                //overrideController["Idle"] = newRunClip;
                anim.SetTrigger("PunchR");
            }
            else if (finger.screenPosition.x < Screen.width / 2)
            {
                Debug.Log("Punch Left");
                leftFist.GetComponent<PlayerFist>().isHitboxActive = true;
                //rb.AddForce(new Vector3(-10,0,0));
                StopAllCoroutines();
                StartCoroutine(Punch()); 
                anim.SetTrigger("PunchL");

            }
            else
            {

            }
        }
    }
    public void Return()
    {
        leftFist.GetComponent<PlayerFist>().isHitboxActive = false;
        rightFist.GetComponent<PlayerFist>().isHitboxActive = false;
        moveState = MoveState.Neutral;
        standingPos = Vector3.zero;
    }
    public IEnumerator Punch()
    {
        moveState = MoveState.Punch;

        speed = 20;
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.forward * 0.5f;
        yield return new WaitForSecondsRealtime(dodgeRecovery * 0.5f);
        Return();

    }
    public IEnumerator DodgeLeft()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        OnDodgeSuccess();
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.right*-1;
        yield return new WaitForSecondsRealtime(dodgeRecovery);
        Return();

    }
    public IEnumerator DodgeRight()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        OnDodgeSuccess();
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.right; 
        yield return new WaitForSecondsRealtime(dodgeRecovery);
        Return();
    }
    public IEnumerator DodgeBack()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        OnDodgeSuccess();
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.back;
        yield return new WaitForSecondsRealtime(dodgeRecovery);
        Return();
    }
}
