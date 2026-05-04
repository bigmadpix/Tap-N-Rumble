using UnityEngine;
using TMPro;

public class ShopCode : MonoBehaviour
{
    public int Tracker = 1;
    public GameObject[] Gloves = new GameObject[2];
    public int GP; 
    public TextMeshProUGUI GPText;
    public TextMeshProUGUI buyOrEquip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetColor();
        Tracker = V.currEquipped;
        GP = V.getGP;
    }

    // Update is called once per frame
    void Update()
    {
        V.getGP = GP;
        GPText.text = "GP: " + GP;
        if(V.Purchased[Tracker-1] == true)
        {
            if (V.currEquipped == Tracker)
            {
                buyOrEquip.text = "Equipped";
            }
            else
            {
                buyOrEquip.text = "Equip";
            }

        }
        else
        {
            buyOrEquip.text = "Buy (5GP)";
        }
    }

    public void ColorTracker(int num)
    {
        Tracker += num;
        if (Tracker > 5) Tracker = 1;
        if(Tracker <= 0) Tracker = 5;
        SetColor();
        Debug.Log(Tracker);
    }

    public void SetColor()
    {
        switch (Tracker)
        {
            case 1:
                foreach (GameObject gloves in Gloves) { 
                    gloves.GetComponent<Renderer>().material.color = Color.grey;

                }
                break;
            case 2:
                foreach (GameObject gloves in Gloves) { 
                    gloves.GetComponent<Renderer>().material.color = Color.red;
                }
                break;
            case 3:
                foreach (GameObject gloves in Gloves) {
                    gloves.GetComponent<Renderer>().material.color = Color.blue;
                }
                break;
            case 4:
                foreach (GameObject gloves in Gloves) {
                    gloves.GetComponent<Renderer>().material.color = Color.yellow;
                }
                break;
            case 5:
                foreach (GameObject gloves in Gloves) { 
                    gloves.GetComponent<Renderer>().material.color = Color.green;
                }
                break;

        }

    }

    public void EquipColor()
    {
        switch (Tracker)
        {
            case 1:
                if(V.Purchased[Tracker -1] == true)
                {
                    V.GloveColor = Color.grey;
                    V.currEquipped = Tracker;
                }
                else
                {
                    Purchase();
                }


                break;
            case 2:
                if (V.Purchased[Tracker - 1] == true)
                {
                    V.GloveColor = Color.red;
                    V.currEquipped = Tracker;
                }
                else
                {
                    Purchase();
                }
                break;
            case 3:
                if (V.Purchased[Tracker - 1] == true)
                {
                    V.GloveColor = Color.blue;
                    V.currEquipped = Tracker;
                }
                else
                {
                    Purchase();
                }
                break;
            case 4:
                if (V.Purchased[Tracker - 1] == true)
                {
                    V.GloveColor = Color.yellow;
                    V.currEquipped = Tracker;
                }
                else
                {
                    Purchase();
                }
                break;
            case 5:
                if (V.Purchased[Tracker - 1] == true)
                {
                    V.GloveColor = Color.green;
                    V.currEquipped = Tracker;
                }
                else
                {
                    Purchase();

                }
                break;

        }
        foreach (GameObject gloves in Gloves)
        {
            Debug.Log(V.GloveColor + "VS " + gloves.GetComponent<Renderer>().material.color);
        }
    }
    public void Purchase()
    {
        if (GP >= 5)
        {
            removeGP();
            V.Purchased[Tracker - 1] = true;
        }
    }
    public void addGP() {
        GP += 1;
    }

    public void removeGP() { GP -= 5; }

}
