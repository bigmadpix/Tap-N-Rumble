using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerData playerDat;

    public static GameManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerData(Player player)
    {
        playerDat.currHP = player.GetHP();
    }
}
[Serializable]
public class PlayerData
{
    public float maxHP;
    public float currHP;
    public List<Perk> perks;
}
