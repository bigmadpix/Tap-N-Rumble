using UnityEngine;

public class ShopCode : MonoBehaviour
{
    int Tracker = 1;
    public GameObject[] Gloves = new GameObject[2];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                foreach (GameObject gloves in Gloves) { gloves.GetComponent<Renderer>().material.color = Color.grey; }
                break;
            case 2:
                foreach (GameObject gloves in Gloves) { gloves.GetComponent<Renderer>().material.color = Color.red; }
                break;
            case 3:
                foreach (GameObject gloves in Gloves) { gloves.GetComponent<Renderer>().material.color = Color.blue; }
                break;
            case 4:
                foreach (GameObject gloves in Gloves) { gloves.GetComponent<Renderer>().material.color = Color.yellow; }
                break;
            case 5:
                foreach (GameObject gloves in Gloves) { gloves.GetComponent<Renderer>().material.color = Color.green; }
                break;

        }

    }

    public void EquipColor()
    {
        switch (Tracker)
        {
            case 0:
                V.GloveColor = Color.grey;
                break;
            case 1:
                V.GloveColor = Color.red;
                break;
            case 2:
                V.GloveColor = Color.blue;
                break;
            case 3:
                V.GloveColor = Color.yellow;
                break;
            case 4:
                V.GloveColor = Color.green;
                break;

        }
        Debug.Log(V.GloveColor);
    }

}
