using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Canvas mapCanvas;
    public static LevelManager instance;
    public LevelNode currentNode;
    public GameObject winCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMap(bool toggle)
    {
        //Camera.main.enabled = toggle;
        mapCanvas.enabled = toggle;
    }
    public void GoMenu()
    {
        if (SceneManager.GetSceneByName("RedEnemyLevel").IsValid())
        {
            SceneManager.UnloadSceneAsync("RedEnemyLevel");
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void FinishCombat()
    {
        SceneManager.UnloadSceneAsync("RedEnemyLevel");
        ToggleMap(true);

        if (currentNode.nodeType == LevelNode.NodeType.Boss) { 
        
            winCanvas.SetActive(true);
        }
    }
}
