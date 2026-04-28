using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class ShopPerk : MonoBehaviour
{
    public Perk currPerk;
    public TextMeshProUGUI perkHeader; 
    public TextMeshProUGUI perkType;
    public Image perkIMG;

    public GameObject perkUI;
    public GameObject soldOutUI;

    public TextMeshProUGUI perkCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PopulateData();
        PerkShopManager.OnRefresh += PopulateData;
        PerkShopManager.OnClose += Refund;
    }

    // Update is called once per frame
    void Update()
    {
        if (currPerk != null)
        {
            soldOutUI.SetActive(false);
            perkUI.SetActive(true);

            perkHeader.text = currPerk.perkName;
            perkType.text = currPerk.perkType.ToString();
            perkCost.text = currPerk.perkCost.ToString();

            GetComponent<Button>().interactable = true;
        }else
        {
            soldOutUI.SetActive(true);
            perkUI.SetActive(false);

            GetComponent<Button>().interactable = false;
        }
    }
    public void PopulateData()
    {
            currPerk = PerkShopManager.instance.GetRandomPerk();

    }
    public void SelectPerk()
    {
        PerkShopManager.instance.selectedPerk = this;
        PerkInspector.instance.InspectPerk(currPerk);
    }
    public void Refund()
    {
        if (currPerk != null)
        {
            PerkShopManager.instance.ReturnPerk(currPerk);
        }
        
    }

}
