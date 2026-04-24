using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EffectManager : MonoBehaviour
{
    public List<Effect> effects = new List<Effect>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Player.OnDodgeSuccess += OnDodgeSuccess;
        Player.OnComboIncrease += OnComboIncrease;
        Player.OnPassive += OnPassive;
        Player.OnKnockout += OnKnockout;
        Player.OnPunch += OnPunch;
        Player.OnPerfectDodge += OnPerfectDodge;
        Player.OnPunchHit += OnPunchHit;
    }
    void OnDisable()
    {
        Player.OnDodgeSuccess -= OnDodgeSuccess;
        Player.OnPerfectDodge -= OnPerfectDodge;
        Player.OnComboIncrease -= OnComboIncrease;
        Player.OnPassive -= OnPassive;
        Player.OnKnockout -= OnKnockout;
        Player.OnPunch -= OnPunch;
        Player.OnPunchHit -= OnPunchHit;
    }

    // Update is called once per frame
    void Update()
    {

        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {
                if(effect.isTimed)
                    effect.DecreaseTimer();

            }
        }
        if(effects.Count != Player.instance.perks.Count)
        {
            LoadBuffs();
        }
    }
    private void Start()
    {

    }

    private void LoadBuffs()
    {
        Debug.Log("Loading New Buffs!");

        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {
                effect.EndEffect();
            }
        }

        effects = new List<Effect>();
        foreach (Perk perk in Player.instance.perks)
        {
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
    public void OnPerfectDodge()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnPerfectDodge();

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
    public void OnPunchHit()
    {
        if (effects.Count > 0)
        {
            foreach (Effect effect in effects)
            {

                effect.OnPunchHit();

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
    public bool isTimed = false; private protected bool isActive = false;
    protected static int effectTimer;
    public Effect()
    {

    }
    public void OnEnable()
    {

    }
    public void OnDisable()
    {

    }
    public bool IsTimed()
    {
        return isTimed;
    }
    public virtual void DecreaseTimer()
    {
        if (effectTimer <= 0) 
        { 
            effectTimer = 0;
            Debug.Log("End timer!");
            EndEffect();
            isActive = false;
            isTimed = false;
            
            return; 
        }
        else
        {
            effectTimer--;

        }
            
    }
    public virtual void SetEffectTimer(float seconds) { 
        isTimed = true;
        effectTimer = Mathf.RoundToInt(seconds*50);
        isActive = true;
        StartEffect();
    }

    public virtual void ResetEffect() { }
    public virtual void OnDodgeSuccess() { }
    public virtual void OnPerfectDodge() { }
    public virtual void OnComboIncrease() { }
    public virtual void OnPassive() { }
    public virtual void OnKnockout() { }
    public virtual void OnPunch() { }
    public virtual void OnPunchHit() { }
    public abstract void StartEffect();
    public virtual void EndEffect() { }
}