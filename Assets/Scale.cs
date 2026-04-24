using UnityEngine;

public class Scale : MonoBehaviour
{
    public float baseScale = 0.01f; // Adjust this to fit your node size

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);


        transform.localScale = Vector3.one * distance * baseScale;

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}