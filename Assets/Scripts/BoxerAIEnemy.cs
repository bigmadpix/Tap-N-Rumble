using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Experimental.GraphView.GraphView;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
public class BoxerAIEnemy : MonoBehaviour
{
    [SerializeField] private string type;
    public float stamina, speed, damage, atkSpeed, defSpeed, percentDodge, percentBlock, Points, dodgeTime;
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
    [SerializeField] private float ChargeUp;


    public GameObject leftGlove, rightGlove;
    [SerializeField] private Transform lG, rG;
    public Player Player;
    private Rigidbody rb;
    public EnemyState ES;
    public Difficulty DF;
    public Animator anim;
    public AnimatorOverrideController aoc;


    public float attackSpeed = 1f;

    public bool isPunchL = false;
    public bool isPunchR = false;


    public bool patternStarted, dodgeStarted, blockStarted;
    
    public enum EnemyState
    {
        Punch,
        Dodge,
        Block,
        tookDamage
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Practice
    }

    //public Animator anim;
    //public AnimatorOverrideController controller;
    private void Awake()
    {
        
        switch (type){ 
            case "Attack":
                stamina = 200;
                damage = 10;
                percentDodge = 0.60f;
                dodgeTime = 3f;
                percentBlock = 0.40f;
                enemyPatterns = Resources.Load<TextAsset>("AttackEnemy Patterns");
                patterns = enemyPatterns.text.Split("\n");                
                Debug.Log("New Enemy: ATTACK");
                break;
            case "Tank":
                stamina = 400;
                damage = 25;
                percentDodge = 0.20f;
                dodgeTime = 3f;
                percentBlock = 0.50f;
                enemyPatterns = Resources.Load<TextAsset>("TankEnemy Patterns");
                patterns = enemyPatterns.text.Split("\n");
                ChargeUp = 0f;
                Debug.Log("New Enemy: TANK");
                break;
            case "Quick":
                stamina = 100;
                damage = 15;
                percentDodge = 0.80f;
                dodgeTime = 5f;
                percentBlock = 0.10f;
                enemyPatterns = Resources.Load<TextAsset>("QuickEnemy Patterns"); 
                textTest = enemyPatterns.text;
                patterns = enemyPatterns.text.Split("\n");
                ChargeUp = 0f;
                Debug.Log("New Enemy: QUICK");
                break;
            default:
                stamina = 200;
                damage = 10;
                percentDodge = 0.60f;
                dodgeTime = 3f;
                percentBlock = 0.40f;
                enemyPatterns = Resources.Load<TextAsset>("AttackEnemy Patterns");
                patterns = enemyPatterns.text.Split("\n");
                Debug.Log("New Enemy: ATTACK");
                break;
        }

        switch (DF)
        {
            case Difficulty.Easy:
                damage -= 5;
                atkSpeed = 0.5f;
                break;
            case Difficulty.Normal:
                damage += 0;
                atkSpeed = 1f;
                break;
            case Difficulty.Hard:
                damage += 5f;
                atkSpeed = 1.5f;
                break;
            case Difficulty.Practice:
                damage = 1;
                stamina = 9999;
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
        blockStarted = false;
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
        Player.instance.OnPunchLand();

    }

    public void Punch(string p)
    {
        Debug.Log("Enemy makes a punch      " + p);
        //speed = 20f;

        anim.speed = atkSpeed;
        switch (p)
        {
            case "L":
                leftGlove.GetComponent<EnemyFist>().ColliderOn = true;
                anim.SetFloat("Speed",attackSpeed);
                anim.SetTrigger("PunchL");
                transform.localScale = new Vector3(1, 1, 1f);
                isPunchL = true;
                isPunchR = false;
                break;
            case "R":
                rightGlove.GetComponent<EnemyFist>().ColliderOn = true;
                anim.SetFloat("Speed", attackSpeed);
                anim.SetTrigger("PunchR");
                transform.localScale = new Vector3(1, 1, -1f);
                isPunchL = false;
                isPunchR = true;
                break;
            default:
                break;
        }
        anim.speed = 1;
        //yield return new WaitForSeconds(0.5f);
        //ES = EnemyState.Default;

        //speed = 10f;
        //nextPunch++;
    }

    public IEnumerator attackPattern(string[] ap)
    {
        isPunchL = false;
        isPunchR = false;
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
                    yield return new WaitForSeconds(0.5f * (1+(1-attackSpeed)));
                }
                if (M == "D")
                {
                    isPunchL = false;
                    isPunchR = false;
                    StartCoroutine("dodgePhase", dodgeTime);
                    yield return new WaitForSeconds(0.2f);
                }
                if (M == "B")
                {
                    StartCoroutine("blockPhase", 5);
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
        isPunchL = false;
        isPunchR = false;
        next = 0;
        ChargeUp += 20f;
        patternStarted = false;
    }

    public IEnumerator dodge(float dir)
    {
        Debug.Log("Enemy Dodged");
        //ES = EnemyState.Dodge;
        boxerPos += Vector3.left * dir;
        yield return new WaitForSeconds(0.5f);
        boxerPos += Vector3.left * -dir;
        dodgeStarted = false;
        //ChargeUp += 10;
        

    }

    public IEnumerator dodgePhase(float time)
    {
        ES = EnemyState.Dodge;
        Debug.Log($"{time} dodge phase");
        yield return new WaitForSeconds(time);
        nextPunch++;
        next = 0;
        ES = EnemyState.Punch;
        Debug.Log("Dodge over");
    }

    public IEnumerator blockPhase(float sec)
    {
        ES = EnemyState.Block;
        GetComponent<Collider>().enabled = false;
        leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
        rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
        yield return new WaitForSeconds(sec);
        nextPunch++;
        next = 0;
        ES = EnemyState.Punch;
        GetComponent<Collider>().enabled = true;
        ES = EnemyState.Punch;
        Debug.Log("Block over");
    }

    //public IEnumerator blockPhase()
    //{
    //    ES = EnemyState.Block;
    //    yield return new WaitForSeconds(3f);
    //    nextPunch++;
    //    next = 0;
    //    ES = EnemyState.Punch;
    //}


    public IEnumerator Wait(float delay)
    {

        yield return new WaitForSeconds(delay);

    }

    public IEnumerator changePercent(float percent)
    {
        yield return new WaitForSeconds(3);
        percentDodge = Random.value;
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
        
        if (Player != null)
        {
            if (ES == EnemyState.tookDamage)
            {
                StartCoroutine(tookDamage());
            }

            if (ES == EnemyState.Punch)
            {
                //string[] curPatt = new string[1];
                if (!patternStarted)
                {
                    switch (type)
                    {
                        case "Attack":
                            string[] curPatt = patterns[nextPunch % (patterns.Length - 1)].Split(',');
                            next = 0;
                            StartCoroutine("attackPattern", curPatt);
                            patternStarted = true;
                        break;
                        case "Tank":
                            if (ChargeUp <= 100f)
                            {
                                curPatt = patterns[nextPunch % 6].Split(',');
                                next = 0;
                                StartCoroutine("attackPattern", curPatt);
                                patternStarted = true;
                            }
                            else
                            {
                                curPatt = patterns[Random.Range(6, 9)].Split(",");
                                next = 0;
                                StartCoroutine("attackPattern", curPatt);
                                patternStarted = true;
                                ChargeUp = 0;
                            }
                        break;
                        case "Quick":
                            if (ChargeUp <= 100f)
                            {
                                curPatt = patterns[nextPunch % 7].Split(',');
                                next = 0;
                                StartCoroutine("attackPattern", curPatt);
                                patternStarted = true;
                            }
                            else
                            {
                                curPatt = patterns[Random.Range(7, 11)].Split(",");
                                next = 0;
                                StartCoroutine("attackPattern", curPatt);
                                patternStarted = true;
                                ChargeUp = 0;
                            }
                            break;
                    }
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

            if (ES == EnemyState.Block)
            {
                anim.SetBool("Block", true);
            }
            else
            {
                anim.SetBool("Block", false);
            }

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

            //if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            //{
            //    anim.enabled = true;
            //    StopAllCoroutines();
            //    string M = curPatt[next];
            //    if (!string.IsNullOrEmpty(M))
            //    {
            //        if (M == "L" || M == "R")
            //        {
            //            //StartCoroutine("Punch", patt);
            //            Punch(M);
            //        }
            //        if (M == "D")
            //        {
            //            ES = EnemyState.Dodge;
            //            StartCoroutine("dodgePhase", dodgeTime);
            //            return;
            //        }
            //        leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
            //        rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
            //    }
            //    //else
            //    //{
            //    //    nextPunch++;
            //    //}
            //    next++;
            //}

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
