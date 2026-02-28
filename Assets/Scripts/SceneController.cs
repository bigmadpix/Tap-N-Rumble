using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject LevelSelectScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenuScreen.SetActive(true);
        LevelSelectScreen.SetActive(false);
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
}
