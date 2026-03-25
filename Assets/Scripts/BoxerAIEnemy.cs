using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxerAIEnemy : MonoBehaviour
{
    public float stamina, speed, damage, percentKO, Points;
    Vector3 boxerPos;
    private EnemySpawn Spawn;
    //[SerializeField] TextMesh textMesh;

    public GameObject leftGlove, rightGlove;
    [SerializeField] private Transform lG, rG;
    public GameObject Player;
    private Rigidbody rb;
    public Animator anim;
    public AnimatorOverrideController controller;
    private void Awake()
    {
        stamina = 100;
        speed = 10f;
        damage = 10;
        percentKO = 0.1f;
        Points = 500;
        boxerPos = transform.position;
        leftGlove = lG.gameObject;
        rightGlove = rG.gameObject;
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        Spawn = FindFirstObjectByType<EnemySpawn>();

        anim = GetComponent<Animator>();
        controller = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = controller;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GetPunched(float p)
    {
        stamina -= p;
        Debug.Log("Enemy lost " +p+ " stamina.");
    }

    public void Punch(GameObject fist)
    {
        fist.GetComponent<Rigidbody>().MovePosition(Player.GetComponent<Transform>().position);
        //fist.GetComponent<Rigidbody>().MovePosition(Player.GetComponent<Transform>().position);
    }

    public void dodge(float dir)
    {
       rb.MovePosition(Player.GetComponent<Transform>().position);
    }

    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stamina <= 0)
        {
            Spawn.SendMessage("AddPoints", Points);
            Destroy(this.gameObject);
        }
        else
        {

        }
        if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
        {
            anim.SetTrigger("PunchL");
            transform.localScale = new Vector3(1, 1, 1f);
        }

        if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
        {
            anim.SetTrigger("PunchR");
            transform.localScale = new Vector3(1,1,-1f);
        }
    }
}
