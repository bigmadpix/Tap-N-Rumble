using UnityEngine;

public class TimeOut : Effect
{
    public TimeOut() { 
    
        effectName = "Time Out";
        effectDescription = "Survive 1 KO once per run. Breaks upon use.";
        StartEffect();
    }

    public override void StartEffect()
    {
        Debug.Log("[TimeOut] is active!");
        isActive = true;
    }

    public override void OnPassive()
    {

        if (isActive) { 
            
            Player.instance.deathPrevention = true;
        
        }
    }
    public override void OnKnockout()
    {
        Player.instance.perks.Remove(parentPerk);
        isActive = false;
    }
    public override void EndEffect()
    {
        Debug.Log("[TimeOut] has ended!");
        isActive = true;
    }
}
