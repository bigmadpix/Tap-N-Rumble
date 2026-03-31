using UnityEngine;

public class EnemyFist : MonoBehaviour
{
    public bool ColliderOn = true;
    [SerializeField] private float damage;
    [SerializeField] private BoxerAIEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<BoxerAIEnemy>();
        damage = enemy.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (ColliderOn) { 
                Debug.Log("Player got punched");
                other.gameObject.GetComponent<Player>().SendMessage("PlayerGotPunched");
                ColliderOn = false;
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
