using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxerAIEnemy : MonoBehaviour
{
    public float stamina, speed, damage, percentDodge, Points;
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
        Dodge,
        tookDamage
    }

    private void Awake()
    {
        stamina = 100;
        speed = 10f;
        damage = 10;
        percentDodge = 0.30f;
        Points = 500;
        boxerPos = transform.position;
        leftGlove = lG.gameObject;
        rightGlove = rG.gameObject;
        Player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        Spawn = FindFirstObjectByType<EnemySpawn>();
        ES = EnemyState.Default;
        enemyMat = GetComponent<Renderer>().material;
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

    public IEnumerator Punch(GameObject fist)
    {
        fist.GetComponent<Rigidbody>().MovePosition(Player.GetComponent<Transform>().position);
        //fist.GetComponent<Rigidbody>().MovePosition(Player.GetComponent<Transform>().position);
        yield return null;
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
            if (Player.moveState == Player.MoveState.Punch && ES != EnemyState.Dodge && ES != EnemyState.tookDamage)
            {
                float r = Random.value;
                if (r <= percentDodge)
                {
                    Debug.Log("Random value: "+r);
                    StartCoroutine("dodge", 3f);
                    //StartCoroutine(changePercent(r));
                }
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
