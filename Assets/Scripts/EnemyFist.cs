using UnityEngine;

public class EnemyFist : MonoBehaviour
{
    public bool ColliderOn = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (ColliderOn) { 
                Debug.Log("Player got punched");
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
