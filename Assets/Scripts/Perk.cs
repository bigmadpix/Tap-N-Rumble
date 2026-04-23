using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Tap-N-Rumble/Perk")]
public class Perk : ScriptableObject
{
    public string perkName;
    public string perkDescription;
    public PerkType perkType;
    public MonoScript perkEffect;

    public Effect GetEffect()
    {
        if (perkEffect == null)
        {
            //Debug.LogAssertion("This effect has no script attached to it!");
            return null;
        }
        var type = typeof(Effect);

        var effectTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var effectType in effectTypes)
        {

                if (perkEffect != null && effectType.Name == perkEffect.GetClass().Name)
                {
                    return (Effect)Activator.CreateInstance(effectType);
                }
        
        }

        return null;
    }
}
    public enum PerkType
    {
        None,
        Timing,
        Survival,
        Resource,
        Wildcard
    }


    
    

