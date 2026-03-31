using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Debug = UnityEngine.Debug;

public class BoxerAIEnemy : MonoBehaviour
{
    [SerializeField] private string type;
    public float stamina, speed, damage, percentDodge, percentBlock, Points, dodgeTime;
    static int nextPunch = 0, next = 0;
    Vector3 boxerPos, prev;
    private EnemySpawn Spawn;
    //[SerializeField] TextMesh textMesh;
    public float deltaX;
    public Material enemyMat;
    private StreamReader sr;
    [SerializeField] private TextAsset enemyPatterns;
    [SerializeField] private string[] patterns;
    [SerializeField] private List<string[]> moveList = new List<string[]>();
    [SerializeField] private string textTest;


    public GameObject leftGlove, rightGlove;
    [SerializeField] private Transform lG, rG;
    public Player Player;
    private Rigidbody rb;
    public EnemyState ES;
    public Animator anim;
    public AnimatorOverrideController aoc;

    public bool patternStarted, dodgeStarted;
    
    public enum EnemyState
    {
        Default,
        Punch,
        Dodge,
        Block,
        tookDamage
    }

    //public Animator anim;
    //public AnimatorOverrideController controller;
    private void Awake()
    {
        int i = Random.Range(0, 3);
        switch (i){ 
            case 0:
                type = "Attack";
                stamina = 200;
                damage = 10;
                percentDodge = 0.60f;
                dodgeTime = 3f;
                percentBlock = 0.40f;
                enemyPatterns = Resources.Load<TextAsset>("AttackEnemy Patterns");
                patterns = enemyPatterns.text.Split("\n");                
                Debug.Log("New Enemy: ATTACK");
                break;
            case 1:
                type = "Tank";
                stamina = 400;
                damage = 25;
                percentDodge = 0.20f;
                dodgeTime = 5f;
                percentBlock = 0.50f;
                enemyPatterns = Resources.Load<TextAsset>("AttackEnemy Patterns");
                patterns = enemyPatterns.text.Split("\n");
                Debug.Log("New Enemy: TANK");
                break;
            case 2:
                type = "Quick";
                stamina = 100;
                damage = 15;
                percentDodge = 0.80f;
                dodgeTime = 7f;
                percentBlock = 0.10f;
                enemyPatterns = Resources.Load<TextAsset>("AttackEnemy Patterns"); 
                textTest = enemyPatterns.text;
                patterns = enemyPatterns.text.Split("\n");
                Debug.Log("New Enemy: QUICK");
                break;
        }
        Points = 500;
        speed = 10f;
        boxerPos = transform.position;
        leftGlove = lG.gameObject;
        rightGlove = rG.gameObject;
        Player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        Spawn = FindFirstObjectByType<EnemySpawn>();

        anim = GetComponent<Animator>();
        ES = EnemyState.Punch;
        enemyMat = GetComponent<Renderer>().material;

        patternStarted = false;
        dodgeStarted = false;
        //foreach(string pattern in patterns)
        //{
        //    moveList.Add(pattern.Split(","));
        //}

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Return()
    {

    }

        

    public void GetPunched(float p)
    {
        stamina -= p;
        Debug.Log("Enemy lost " + p + " stamina.");
        percentDodge -= 0.05f;
        ES = EnemyState.tookDamage;

    }

    //public void Punch(float f)
    //{
    //    ES = EnemyState.Punch;
    //    Debug.Log("Enemy makes a punch");
    //    speed = 20f;
    //    //boxerPos = Vector3.forward * -0.5f;
    //    //boxerPos = Vector3.forward * 0.5f;

    //    if (nextPunch % 2 == 0)
    //    {
    //        leftGlove.GetComponent<EnemyFist>().ColliderOn = true;
    //        anim.SetTrigger("PunchL");
    //        transform.localScale = new Vector3(1, 1, 1f);
            
    //    }
    //    else
    //    {
    //        rightGlove.GetComponent<EnemyFist>().ColliderOn = true;
    //        anim.SetTrigger("PunchR");
    //        transform.localScale = new Vector3(1, 1, -1f);
    //    }

    //    //yield return new WaitForSeconds(f);
    //    ES = EnemyState.Default;

    //    speed = 10f;
    //    nextPunch++;
    //}

    //public IEnumerator Punch(string p)
    //{
    //    Debug.Log("Enemy makes a punch      " + p);
    //    speed = 20f;
    //    //boxerPos = Vector3.forward * -0.5f;
    //    //boxerPos = Vector3.forward * 0.5f;
    //    if (anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchL") || anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchR"))
    //    {
    //        anim.SetTrigger("Idle");
    //    }

    //    switch (p)
    //    {
    //        case "L":
    //            leftGlove.GetComponent<EnemyFist>().ColliderOn = true;
    //            anim.SetTrigger("PunchL");
    //            transform.localScale = new Vector3(1, 1, 1f);
    //            break;
    //        case "R":
    //            rightGlove.GetComponent<EnemyFist>().ColliderOn = true;
    //            anim.SetTrigger("PunchR");
    //            transform.localScale = new Vector3(1, 1, -1f);
    //            break;
    //        default:
    //            break;
    //    }

    //    yield return new WaitForSeconds(0.5f);
    //    //ES = EnemyState.Default;

    //    speed = 10f;
    //}

    public void Punch(string p)
    {
        //ES = EnemyState.Punch;
        Debug.Log("Enemy makes a punch      " + p);
        speed = 20f;
        //boxerPos = Vector3.forward * -0.5f;
        //boxerPos = Vector3.forward * 0.5f;
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchL") || anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchR"))
        //{
        //    anim.SetTrigger("Idle");
        //    return;
        //}

        switch (p)
        {
            case "L":
                leftGlove.GetComponent<EnemyFist>().ColliderOn = true;
                anim.SetTrigger("PunchL");
                transform.localScale = new Vector3(1, 1, 1f);
                break;
            case "R":
                rightGlove.GetComponent<EnemyFist>().ColliderOn = true;
                anim.SetTrigger("PunchR");
                transform.localScale = new Vector3(1, 1, -1f);
                break;
            default:
                break;
        }

        //yield return new WaitForSeconds(0.5f);
        //ES = EnemyState.Default;

        speed = 10f;
        //nextPunch++;
    }

    public IEnumerator attackPattern(string[] ap)
    {
        yield return new WaitForSeconds(1f);
        anim.enabled = true;
        for (int i = 0; i < ap.Length; i++)
        {
            string M = ap[i];
            if (!string.IsNullOrEmpty(M))
            {
                if (M == "L" || M == "R")
                {
                    //StartCoroutine("Punch", patt);
                    Punch(M);
                    yield return new WaitForSeconds(0.5f);
                }
                if (M == "D")
                {
                    StartCoroutine("dodgePhase", dodgeTime);
                    yield return new WaitForSeconds(0.2f);
                }
                leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
                rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
            }
        }
        //else
        //{
        //    nextPunch++;
        //}
        
        yield return new WaitForSeconds(1f);
        next = 0; 
        patternStarted = false;
        ES = EnemyState.Dodge;
    }

    public IEnumerator dodge(float dir)
    {
        Debug.Log("Enemy Dodged");
        //ES = EnemyState.Dodge;
        boxerPos += Vector3.left * dir;
        yield return new WaitForSeconds(0.5f);
        boxerPos += Vector3.left * -dir;
        dodgeStarted = false;
        //ReturnToDef();
        //yield return new WaitForSeconds(1);
    }

    public IEnumerator dodgePhase(float time)
    {
        Debug.Log($"{time} dodge phase");
        yield return new WaitForSeconds(time);
        nextPunch++;
        next = 0;
        ES = EnemyState.Punch;
        Debug.Log("Dodge over");
    }

    public IEnumerator block()
    {
        ES = EnemyState.Block;
        yield return new WaitForSeconds(1);
        ES = EnemyState.Default;
    }

    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);

    }

    public IEnumerator changePercent(float percent)
    {
        yield return new WaitForSeconds(3);
        percentDodge = Random.value;
    }

    public void ReturnToDef()
    {
        ES = EnemyState.Default;
    }

    public IEnumerator tookDamage()
    {
        enemyMat.color = new Color(1f, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(1);
        enemyMat.color = Color.gray;
        ES = EnemyState.Punch;
        //ReturnToDef();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPos = transform.position - prev;
        deltaX = deltaPos.x;

        prev = transform.position;

        Vector3 target = Vector3.Lerp(transform.position, (boxerPos), Time.deltaTime * speed);
        transform.position = new Vector3(target.x, transform.position.y, target.z);

        string[] curPatt = patterns[nextPunch % (patterns.Length - 1)].Split(',');

        //if (Player != null)
        //{
        //    if (ES == EnemyState.Default)
        //    {
        //        StopAllCoroutines();
        //        if (Player.moveState == Player.MoveState.Punch)
        //        {
        //            float DG = Random.value;
        //            if (DG >= percentDodge)
        //            {
        //                Debug.Log("Random value: " + DG);
        //                if(Random.value <= 0.5f)
        //                    StartCoroutine("dodge", 3f);
        //                else 
        //                    StartCoroutine("dodge", -3f);
        //                //StartCoroutine(changePercent(r));
        //            }
        //        }
        //        else
        //        {
        //            float r = Random.Range(0.5f, 3f);
        //            StartCoroutine("Punch", r);
        //        }
        //        leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
        //        rightGlove.GetComponent<EnemyFist>().ColliderOn = false;

        //    }
        if (Player != null)
        {
            if (ES == EnemyState.tookDamage)
            {
                StartCoroutine(tookDamage());
            }

            if (ES == EnemyState.Punch)
            {
                if (!patternStarted)
                {
                    next = 0;
                    StartCoroutine("attackPattern", curPatt);
                    patternStarted = true;
                }

                //foreach (string patt in curPatt)
                //{
                //    StopAllCoroutines();
                //    if (!string.IsNullOrEmpty(patt))
                //    {
                //        if (patt == "L" || patt == "R")
                //        {
                //            //StartCoroutine("Punch", patt);
                //            Punch(patt);
                //        }
                //        if (patt == "D")
                //        {
                //            ES = EnemyState.Dodge;
                //            StartCoroutine("dodgePhase", dodgeTime);
                //            return;
                //        }
                //        leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
                //        rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
                //    }
                //}
            }

            if (ES == EnemyState.Dodge)
            {
                
                if (Player.moveState == Player.MoveState.Punch && !dodgeStarted)
                {
                    float DG = Random.value;
                    Debug.Log(DG);
                    if (DG >= percentDodge)
                    {
                        dodgeStarted = true;
                        Debug.Log("Random value: " + DG);
                        if (Random.value <= 0.5f)
                            StartCoroutine("dodge", 3f);
                        else
                            StartCoroutine("dodge", -3f);
                        //StartCoroutine(changePercent(r));
                    }
                }
            }
            //}

            if (stamina <= 0)
            {
                anim.enabled = false;
                rb.useGravity = true;
                Spawn.SendMessage("AddPoints", Points);
                Destroy(this.gameObject, 2);
            }

            if (dodgeStarted)
            {
                anim.enabled = false;
            }

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                anim.enabled = true;
                StopAllCoroutines();
                string M = curPatt[next];
                if (!string.IsNullOrEmpty(M))
                {
                    if (M == "L" || M == "R")
                    {
                        //StartCoroutine("Punch", patt);
                        Punch(M);
                    }
                    if (M == "D")
                    {
                        ES = EnemyState.Dodge;
                        StartCoroutine("dodgePhase", dodgeTime);
                        return;
                    }
                    leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
                    rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
                }
                //else
                //{
                //    nextPunch++;
                //}
                next++;
            }

            ////LEFT PUNCH TESTING ANIMATIONS
            //if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
            //{
            //    anim.SetTrigger("PunchL");
            //    transform.localScale = new Vector3(1, 1, 1f);
            //}

            ////RIGHT PUNCH TESTING ANIMATIONS
            //if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
            //{
            //    anim.SetTrigger("PunchR");
            //    transform.localScale = new Vector3(1,1,-1f);
            //}
        }
    }

    public string getType()
    {
        return type;
    }

    public float getDamage()
    {
        return damage;
    }

    public float getStamina()
    {
        return stamina;
    }
}
