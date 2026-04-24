using UnityEngine;

public class GlassCannon : Effect
{
    public GlassCannon() { 
    
        effectName = "Glass Cannon";
        effectDescription = "You deal double damage, but you take double damage.";
    
    }
    public override void OnPassive()
    {
        if(!isActive)
            StartEffect();
    }
    public override void StartEffect()
    {
        isActive = true;
        Debug.Log("[GlassCannon] is active!");
        Player.instance.baseDMG = 2f;
        Player.instance.baseVULN = 2f;
    }
    public override void EndEffect()
    {
        isActive = false;
        Debug.Log("[GlassCannon] has ended!");
        Player.instance.baseDMG = 1f;
        Player.instance.baseVULN = 1f;
    }
}
