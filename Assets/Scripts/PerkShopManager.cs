using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PerkShopManager : MonoBehaviour
{
    public ShopPerk selectedPerk;
    public GameObject shopObj;
    public bool isActive = false;
    public static PerkShopManager instance;
    public List<Perk> availablePerks;

    public static event Action OnRefresh;
    public static event Action OnClose;

    public TextMeshProUGUI dollerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        Perk[] allPerks = Resources.LoadAll<Perk>("Perks");
        foreach (Perk perk in allPerks)
        {
            availablePerks.Add(perk);
        }

        
    }

    private void Update()
    {
        dollerText.text = "$"+GameManager.instance.playerDat.dollers.ToString();
    }
    public Perk GetRandomPerk()
    {
        int select = UnityEngine.Random.Range(0, availablePerks.Count);
        Perk selectedPerk = availablePerks[select];
        availablePerks.Remove(selectedPerk);
        return selectedPerk;
    }
    // Update is called once per frame
    public void Close()
    {
        shopObj.SetActive(false);
        OnClose();
    }
    public void Open()
    {
        
        shopObj.SetActive(true); 
        Refresh();
    }
    public void Refresh()
    {
        OnRefresh();
    }
    public void ReturnPerk(Perk perk)
    {
        availablePerks.Add(perk);
    }
    public void Purchase()
    {

        PlayerData player = GameManager.instance.playerDat; 
        if (player.dollers >= selectedPerk.currPerk.perkCost)
        {
            player.dollers -= selectedPerk.currPerk.perkCost;
            player.perks.Add(selectedPerk.currPerk);
            selectedPerk.currPerk = null;

        }
        else
        {
            Debug.Log("Insufficent Dollers!");
        }
        


    }
}
