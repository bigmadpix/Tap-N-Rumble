using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public int tutorialStep = 0;
    public bool isActive = false;

    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            step1.SetActive(tutorialStep == 0);
            step2.SetActive(tutorialStep == 1);
            step3.SetActive(tutorialStep == 2);
            step4.SetActive(tutorialStep == 3);
        }
        else
        {
            step1.SetActive(false);
            step2.SetActive(false);
            step3.SetActive(false);
            step4.SetActive(false);
        }

    }
}
