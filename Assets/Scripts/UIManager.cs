using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class UIManager : MonoBehaviour
{
    public Image damageIndicator;
    public Color damageColor;

    public static UIManager instance;

    public Player player;
    public TextMeshProUGUI playerHPTxt;
    public TextMeshProUGUI playerSPTxt;

    public GameObject gameOverCanvas;

    public int combo;
    public int comboTimer;
    public TextMeshProUGUI comboTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        damageIndicator.color = damageColor;
        damageColor.a -= 0.1f;

        playerHPTxt.text = "HP: " + player.GetHP();
        playerSPTxt.text = "SP: " + player.GetSP();

        if(comboTimer > 0)
        {
            comboTimer--;
            if(combo > 1)
            {
                comboTxt.text = "Combo!: " + combo + "x";
            }

        }
        else
        {
            combo = 0;
            comboTxt.text = "";
        }
    }

    public void HitVFX()
    {
        damageColor.a = 1f;
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        LevelManager.instance.GoMenu();
    }

    public void AddCombo()
    {
        combo++;
        comboTimer += 50;
    }
}
