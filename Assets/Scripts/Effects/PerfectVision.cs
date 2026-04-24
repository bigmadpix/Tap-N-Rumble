using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
public class PerfectVision : Effect
{
    public PerfectVision() { 
    
        effectName = "Perfect Vision";
        effectDescription = "Hindsight is 20/20. Slow time on Dodge.";
    }

    public override void OnPerfectDodge() => SetEffectTimer(3);

    public override void OnPunchHit() => EndEffect();

    public override void StartEffect()
    {
        Debug.Log("[PerfectVision] is active!");
        Time.timeScale = 0.25f;
        Player.instance.isInvincible = true;
    }
    public override void EndEffect()
    {
        Debug.Log("[PerfectVision] has ended!");
        Time.timeScale = 1f;
        Player.instance.isInvincible = false;
    }
}
