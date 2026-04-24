using UnityEngine;

public class FlippedControls : Effect
{
    public FlippedControls() { 
    
        effectName = "Flipped Controls";
        effectDescription = "You enter the southpaw stance. Controls are flipped, but you deal more damage.";
    
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
        Debug.Log("[FlippedControls] is active!");
        isActive = true;
        Player.instance.isSouthpaw = true; 
        Player.instance.playerATKBuff += 0.25f;
    }
    public override void EndEffect()
    {
        Debug.Log("[FlippedControls] has ended!");
        isActive = false;
        Player.instance.isSouthpaw = false; 
        Player.instance.playerATKBuff -= 0.25f;
    }
}
