using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PerkInspector : MonoBehaviour
{
    public Perk currPerk;
    public TextMeshProUGUI perkHeader;
    public TextMeshProUGUI perkType;
    public TextMeshProUGUI perkDescription;
    public TextMeshProUGUI effectName;
    public TextMeshProUGUI effectDescription;

    public static PerkInspector instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (currPerk != null)
        {
            perkHeader.text = currPerk.perkName;
            perkType.text = currPerk.perkType.ToString();
            perkDescription.text = currPerk.perkDescription;
            effectName.text = currPerk.GetEffect().effectName;
            effectDescription.text = currPerk.GetEffect().effectDescription;
        }
    }

    public void InspectPerk( Perk prevPerk)
    {
        currPerk = prevPerk;
    }
}
