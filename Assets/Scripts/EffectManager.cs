using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EffectManager : MonoBehaviour
{
    public List<Effect> effects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Player.OnDodgeSuccess += OnDodgeSuccess;
        Player.OnComboIncrease += OnComboIncrease;
        Player.OnPassive += OnPassive;
        Player.OnKnockout += OnKnockout;
        Player.OnPunch += OnPunch;
    }
    void OnDisable()
    {
        Player.OnDodgeSuccess -= OnDodgeSuccess;
        Player.OnComboIncrease -= OnComboIncrease;
        Player.OnPassive -= OnPassive;
        Player.OnKnockout -= OnKnockout;
        Player.OnPunch -= OnPunch;
    }

    // Update is called once per frame
    void Update()
    {
        effects = new List<Effect>();
        foreach (Perk perk in Player.instance.perks)
        {
            /*
            if(perk != null)
                effects.Add(GetComponent<EffectGetter>().GetEffect(perk.perkEffect));
            */
            if (perk != null && perk.GetEffect() != null)
                effects.Add(perk.GetEffect());
        }
    }

    public void OnDodgeSuccess()
    {
        if(effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnDodgeSuccess();

            }
        }

    }
    public void OnComboIncrease()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnComboIncrease();

            }
        }
    }
    public void OnPassive()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnPassive();

            }
        }
    }
    public void OnKnockout()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnKnockout();

            }
        }
    }
    public void OnPunch()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnPunch();

            }
        }
    }
}
[Serializable]
public abstract class Effect
{
    public string effectName;
    public string effectDescription;
    public Perk parentPerk;

    public Effect()
    {

    }
    public void OnEnable()
    {

    }
    public void OnDisable()
    {

    }

    public virtual void OnDodgeSuccess() { }
    public virtual void OnComboIncrease() { }
    public virtual void OnPassive() { }
    public virtual void OnKnockout() { }
    public virtual void OnPunch() { }
}