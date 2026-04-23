using UnityEngine;

public class TestEffect : Effect
{
    public TestEffect() { 
    
        effectName = "Test";
        effectDescription = "Test";
    
    }

    public override void OnPassive()
    {
        Debug.Log("[Test Effect] is active!");
    }
}
