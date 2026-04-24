using UnityEngine;

public class BulkingSeason : Effect
{
    public BulkingSeason() { 
    
        effectName = "Bulking Season";
        effectDescription = "Punches require more stamina, but deal more damage.";
        
    }
    public override void OnPassive()
    {
        if (!isActive)
        {
            StartEffect();
        }
    }
    public override void StartEffect()
    {
        Debug.Log("[BulkingSeason] is active!");
        isActive = true;
        Player.instance.staminaConsumption = 0.25f;
        Player.instance.playerATKBuff += 0.15f;
    }
    public override void EndEffect()
    {
        Debug.Log("[BulkingSeason] has ended!");
        isActive = false;
        Player.instance.staminaConsumption = 0.1f;
        Player.instance.playerATKBuff -= 0.15f;
    }
}
