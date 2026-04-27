using Unity.VisualScripting;
using UnityEngine;

public class EnemyFist : MonoBehaviour
{
    public bool ColliderOn = true;
    [SerializeField] private float damage;
    [SerializeField] private BoxerAIEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<BoxerAIEnemy>();
        GetComponent<Collider>().enabled = ColliderOn;
    }
    private void Update()
    {
        damage = enemy.damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            if (ColliderOn) {

                if (enemy.ES != BoxerAIEnemy.EnemyState.Block)
                {
                    Debug.Log("Player got punched");
                    other.gameObject.GetComponentInParent<Player>().OnHit(damage);
                    ColliderOn = false;
                }
                
            }
        }
        if (other.gameObject.tag == "Fist")
        {
            if (ColliderOn)
            {
                Debug.Log("Block");
            }
        }
    }
}
