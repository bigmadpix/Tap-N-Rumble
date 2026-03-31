using System.Collections;
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
        rb = GetComponent<Rigidbody>();
        // Keep a note of the time the movement started.
        startTime = Time.time;
        controller = new AnimatorOverrideController(anim.runtimeAnimatorController);
        // Calculate the journey length.
        journeyLength = Vector3.Distance(eyeBox.position, standingPos * 5f);
        anim.runtimeAnimatorController = controller;

        _lowPassValue = Accelerometer.current.acceleration.ReadValue();

    }

    public float GetHP()
    {
        return playerStamina;
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
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, eyeBox.position, Time.deltaTime * camSpeed);
        Camera.main.transform.rotation = eyeBox.rotation;
    }
    void Update()
    {
        if (playerStamina <= 0) {
            playerStamina = 0;
            UIManager.instance.GameOver();
        }
        _timeSinceLastShake += Time.deltaTime;

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

        Vector3 target = Vector3.Lerp(transform.position, (standingPos * 5f), Time.deltaTime * speed);
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
    }

    public void PlayerGotPunched(float p)
    {
 
    }

    public void OnHit(float p)
    {
        playerStamina -= p;
        UIManager.instance.HitVFX();
    }
    public void OnShake()
    {
        Debug.Log("Initiating Super Special Move!!");
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null) { enemy.GetComponent<BoxerAIEnemy>().GetPunched(playerSP); playerSP = 0; } else
        {
            Debug.Log("Attack Missed. No Enemy Found");
        }
        
    }

    public void OnFingerDown(Finger finger)
    {
        startPosTouch = finger.screenPosition;
        //Debug.Log(finger.screenPosition);
    }

    private void OnFingerUp(Finger finger)
    {
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
                    StartCoroutine(DodgeRight());
                }
                else
                {
                    Debug.Log("Swiped Left"); 
                    StopAllCoroutines();
                    StartCoroutine(DodgeLeft());
                    
                }
            }
            else
            {
                // Vertical swipe
                if (swipeDirection.y > 0)
                {
                    Debug.Log("Swiped Up");
                }
                else
                {
                    Debug.Log("Swiped Down");
                    StopAllCoroutines();
                    StartCoroutine(DodgeBack());
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
        yield return new WaitForSeconds(dodgeRecovery * 0.5f);
        Return();

    }
    public IEnumerator DodgeLeft()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.right*-1;
        yield return new WaitForSeconds(dodgeRecovery);
        Return();

    }
    public IEnumerator DodgeRight()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.right; 
        yield return new WaitForSeconds(dodgeRecovery);
        Return();
    }
    public IEnumerator DodgeBack()
    {
        moveState = MoveState.Dodge;
        speed = 5;
        //rb.AddForce(new Vector3(-10,0,0));
        standingPos = Vector3.back;
        yield return new WaitForSeconds(dodgeRecovery);
        Return();
    }
}
