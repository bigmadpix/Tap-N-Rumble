using UnityEngine;
using TMPro;
public class MapUI : MonoBehaviour
{
    public TextMeshProUGUI healthTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthTxt.text = $"HP: {GameManager.instance.playerDat.currHP}%";
    }
}
