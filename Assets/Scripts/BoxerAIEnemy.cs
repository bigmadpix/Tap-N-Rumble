using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BoxerAIEnemy : MonoBehaviour
{
    [SerializeField] private string type;
    public float stamina, speed, damage, percentDodge, percentBlock, Points;
    static int nextPunch = 0;
    Vector3 boxerPos, prev;
    private EnemySpawn Spawn;
    //[SerializeField] TextMesh textMesh;
    public float deltaX;
    public Material enemyMat;

    public GameObject leftGlove, rightGlove;
    [SerializeField] private Transform lG, rG;
    public Player Player;
    private Rigidbody rb;
    public EnemyState ES;
    public Animator animator;
    public AnimatorOverrideController aoc;


    public enum EnemyState
    {
        Default,
        Punch,
        LeftHook,
        RightHook,
        Dodge,
        Block,
        tookDamage
    }

    private void Awake()
    {
        int i = Random.Range(0, 3);
        switch (i){ 
            case 0:
                type = "Attack";
                stamina = 200;
                damage = 10;
                percentDodge = 0.60f;
                percentBlock = 0.40f;
                Debug.Log("New Enemy: ATTACK");
                break;
            case 1:
                type = "Tank";
                stamina = 400;
                damage = 25;
                percentDodge = 0.20f;
                percentBlock = 0.50f;
                Debug.Log("New Enemy: TANK");
                break;
            case 2:
                type = "Quick";
                stamina = 100;
                damage = 15;
                percentDodge = 0.80f;
                percentBlock = 0.10f;
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
        ES = EnemyState.Default;
        enemyMat = GetComponent<Renderer>().material;
        animator = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = aoc;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

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

    public IEnumerator Punch(float f)
    {
        //ES = EnemyState.Punch;
        //Debug.Log("Enemy makes a punch");
        //speed = 20f;
        //boxerPos = Vector3.forward * -0.5f;
        //boxerPos = Vector3.forward * 0.5f;

        //if (nextPunch%2 == 0)
        //{
        //    //leftGlove.GetComponent<EnemyFist>().ColliderOn = true;
        //    //leftGlove.GetComponent<Rigidbody>().AddForce(-10, 0, 0);
        //    //leftGlove.GetComponent<Rigidbody>().AddForce(10, 0, 0);
        //    animator.SetTrigger("PunchL");
        //}
        //else
        //{
        //    //rightGlove.gameObject.GetComponent<EnemyFist>().ColliderOn = true;
        //    //rightGlove.GetComponent<Rigidbody>().AddForce(-10, 0, 0);
        //    //rightGlove.GetComponent<Rigidbody>().AddForce(10, 0, 0);
        //    animator.SetTrigger("PunchR");
        //}

        yield return new WaitForSeconds(f);
        ES = EnemyState.Default;

        speed = 10f;
        nextPunch++;
    }

    public IEnumerator dodge(float dir)
    {
       
        //Debug.Log("Enemy Dodged");
        ES = EnemyState.Dodge;
        boxerPos += Vector3.left * dir;
        yield return new WaitForSeconds(0.5f);
        boxerPos += Vector3.left * -dir;
        ReturnToDef();
        yield return new WaitForSeconds(1);
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
        ReturnToDef();
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
            if (ES == EnemyState.Default)
            {
                StopAllCoroutines();
                if (Player.moveState == Player.MoveState.Punch)
                {
                    float DG = Random.value;
                    if (DG <= percentDodge)
                    {
                        //Debug.Log("Random value: " + DG);
                        if(Random.value <= 0.5)
                            StartCoroutine("dodge", 3f);
                        else 
                            StartCoroutine("dodge", -3f);
                        //StartCoroutine(changePercent(r));
                    }
                }
                //else
                //{
                //    float r = Random.Range(0.5f, 3f);
                //    StartCoroutine("Punch", r);
                //}
                //leftGlove.GetComponent<EnemyFist>().ColliderOn = false;
                //rightGlove.GetComponent<EnemyFist>().ColliderOn = false;
                
            }
        }

        if (ES == EnemyState.tookDamage)
        {
            StartCoroutine(tookDamage());
        }

        if (stamina <= 0)
        {
            rb.useGravity = true;
            Spawn.SendMessage("AddPoints", Points);
            Destroy(this.gameObject, 2);
        }
        else
        {

        }

    }

}
