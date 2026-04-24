using UnityEngine;

public class FocusBreathing : Effect
{
    public FocusBreathing() { 
    
        effectName = "Focus Breathing";
        effectDescription = "Dodging attacks build stamina (HP)";
    
    }
    public override void OnDodgeSuccess() => StartEffect();
    public override void StartEffect()
    {
        Debug.Log("[FocusBreathing] is active!");
        Player.instance.AddHP(Player.instance.GetMaxHP()*0.03125f);
    }
}
