using UnityEngine;

public class PlayerGloves : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Renderer>().material.color = V.GloveColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
