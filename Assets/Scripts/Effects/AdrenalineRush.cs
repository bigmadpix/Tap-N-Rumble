using UnityEngine;

public class AdrenalineRush : Effect
{
    public AdrenalineRush() { 
    
        effectName = "Adrenaline Rush";
        effectDescription = "Low HP increases stamina recovery speed and damage.";
    
    }
    public override void OnPassive()
    {
        if (Player.instance.GetHP() <= (Player.instance.GetMaxHP() * 0.25f)) {
            if (!isActive)
                StartEffect();
        }
        else
        {
            if (isActive)
                EndEffect();
        }
    }
    public override void StartEffect()
    {
        isActive = true;
        Debug.Log("[AdrenalineRush] is active!");
        Player.instance.playerATKBuff += 0.15f;
    }
    public override void EndEffect()
    {
        isActive = false;
        Debug.Log("[AdrenalineRush] has ended!");
        Player.instance.playerATKBuff -= 0.15f;
    }
}
