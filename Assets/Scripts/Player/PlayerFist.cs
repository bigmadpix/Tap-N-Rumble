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
                player.AddCombo();

                other.GetComponent<BoxerAIEnemy>().GetPunched(player.baseDMG*(player.playerATK * (1f + player.playerATKBuff)));
                isHitboxActive = false;
            }
           
        }
    }
}
