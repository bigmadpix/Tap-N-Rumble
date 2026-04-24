using UnityEngine;

public class ComboFlow : Effect
{
    private static float buff = 0f;
    public ComboFlow() { 
    
        effectName = "Combo Flow";
        effectDescription = "Consecutive Combos increases attack damage.";
    
    }
    public override void OnComboIncrease() => SetEffectTimer(3);
    public override void StartEffect()
    {
        Debug.Log("[ComboFlow] is active!");
        buff += 0.1f;
        Player.instance.playerATKBuff += 0.1f;
    }
    public override void EndEffect()
    {
        Debug.Log("[ComboFlow] has ended!");
        Player.instance.playerATKBuff -= buff;
        buff = 0f;
    }
}
