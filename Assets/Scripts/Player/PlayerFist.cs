using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    public bool isHitboxActive;
    private Player player;

    public void Start()
    {
        
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (isHitboxActive)
            {
                Debug.Log("Enemy Hit");
                player.AddSP(10);
                UIManager.instance.AddCombo();
                other.GetComponent<BoxerAIEnemy>().GetPunched(20f);
                isHitboxActive = false;
            }
           
        }
    }
}
