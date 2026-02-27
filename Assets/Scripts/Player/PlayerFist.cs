using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    public bool isHitboxActive;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (isHitboxActive)
            {
                Debug.Log("Enemy Hit");
                other.GetComponent<BoxerAIEnemy>().GetPunched(20f);
                isHitboxActive = false;
            }
           
        }
    }
}
