using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelNode : MonoBehaviour
{
    public TextMeshProUGUI header;
    public NodeType nodeType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum NodeType
    {
        Start, //start level
        Combat,// common enemies
        Elite,// common enemies
        Shop,// if perks
        Rest,// rest and train (level up using money)
        Boss //ends the level
    }
    void Start()
    {
        header = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (nodeType)
        {
            case NodeType.Start:
                header.text = "Start";
                break;
            case NodeType.Combat:
                header.text = "Fight";
                break;
            case NodeType.Elite:
                header.text = "Elite";
                break;
            case NodeType.Shop:
                header.text = "Shop";
                break;
            case NodeType.Rest:
                header.text = "Rest";
                break;
            case NodeType.Boss:
                header.text = "Boss";
                break;

            default:
                break;
        }
    }

    public void OnNodeEnter()
    {
        switch (nodeType)
        {
            case NodeType.Start:
                break;
            case NodeType.Combat:
                LevelManager.instance.ToggleMap(false);
                SceneManager.LoadScene("RedEnemyLevel", LoadSceneMode.Additive);
                break;
            case NodeType.Elite:
                LevelManager.instance.ToggleMap(false);
                SceneManager.LoadScene("RedEnemyLevel", LoadSceneMode.Additive);
                break;
            case NodeType.Shop:
                PerkShopManager.instance.Open();
                break;
            case NodeType.Rest:
                PlayerData player = GameManager.instance.playerDat;
                if (player.currHP < player.maxHP)
                {
                    player.currHP += Mathf.RoundToInt(player.currHP * 0.25f);
                }

                if (player.currHP > player.maxHP)
                {
                    player.currHP = player.maxHP;
                }
                break;
            case NodeType.Boss:
                LevelManager.instance.ToggleMap(false);
                SceneManager.LoadScene("RedEnemyLevel", LoadSceneMode.Additive);
                break;
            default:
                break;
        }
    }
}
