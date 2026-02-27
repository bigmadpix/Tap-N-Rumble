using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxerAIEnemy : MonoBehaviour
{
    public float stamina, speed, damage, percentKO;
    Vector3 boxerPos;

    public GameObject leftGlove, rightGlove;
    [SerializeField] private Transform lG, rG;
    public GameObject Player;
    private Rigidbody rb;

    private void Awake()
    {
        stamina = 100;
        speed = 10f;
        damage = 10;
        percentKO = 0.1f;
        boxerPos = transform.position;
        leftGlove = lG.gameObject;
        rightGlove = rG.gameObject;
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GetPunched(float p)
    {
        stamina -= p;
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
        if(stamina <= 0)
        {
            Wait(2f);
            Destroy(this.gameObject);
        }

    }
}
