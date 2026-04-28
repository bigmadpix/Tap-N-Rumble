using UnityEngine;

public class InputReading : Effect
{
    BoxerAIEnemy enemy;
    public InputReading() { 
    
        effectName = "Input Reading";
        effectDescription = "Read your opponent. Chance for arrows to tell you where to dodge before an enemy attack.";
    }
    public override void OnPassive()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BoxerAIEnemy>();
        if (!isActive)
        {
            StartEffect();
        }
        if (isActive)
        {

            if (enemy != null && (enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime  < 0.7f))
            {
                UIManager.instance.effects_InputReadingArrowL.SetActive(enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchR"));
                UIManager.instance.effects_InputReadingArrowR.SetActive(enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyPunchL"));
            }
            else
            {
                UIManager.instance.effects_InputReadingArrowL.SetActive(false);
                UIManager.instance.effects_InputReadingArrowR.SetActive(false);
            }
        }
    }
    public override void StartEffect()
    {
        Debug.Log("[InputReading] is active!");
        isActive = true;
    }
    public override void EndEffect()
    {
        Debug.Log("[InputReading] has ended!");
        isActive = false;
    }
}
