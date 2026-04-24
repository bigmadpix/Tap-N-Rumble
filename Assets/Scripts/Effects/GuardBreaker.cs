using UnityEngine;

public class GuardBreaker : Effect
{
    public GuardBreaker() { 
    
        effectName = "Guard Breaker";
        effectDescription = "Increase damage against guarding attacks.";
    
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
        Debug.Log("[GuardBreaker] is active!");
        isActive = true;
        Player.instance.guardDamage = 0.75f;
    }
    public override void EndEffect()
    {
        Debug.Log("[GuardBreaker] has ended!");
        isActive = false;
        Player.instance.staminaConsumption = 0.1f;
    }
}
