using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject LevelSelectScreen;
    private Boolean PauseToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MainMenuScreen != null && LevelSelectScreen != null)
        {
            MainMenuScreen.SetActive(true);
            LevelSelectScreen.SetActive(false);
        }
        PauseToggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Will return to the main menu or close the game if in main menu 
    public void Exit()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        if (CurrentScene.name.Equals("MainMenu")) Application.Quit();

        else LevelLoader("MainMenu");
    }

    public void LevelLoader(string Name)
    {
        SceneManager.LoadScene(Name);
    }

    public void MenuToggle()
    {
        MainMenuScreen.SetActive(!MainMenuScreen.activeSelf);
        LevelSelectScreen.SetActive(!LevelSelectScreen.activeSelf);
    }

    public void PauseButton()
    {
        GameObject TrackEnemy = GameObject.FindWithTag("Enemy");
        GameObject Player = GameObject.Find("Player");

        if (PauseToggle != true)
        {
            PauseToggle = true;
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            TrackEnemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            MainMenuScreen.SetActive(true);
        }

        else
        {
            MainMenuScreen.SetActive(false);
            PauseToggle = false; 
            Player.GetComponent<Rigidbody>().freezeRotation = false;
            TrackEnemy.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
