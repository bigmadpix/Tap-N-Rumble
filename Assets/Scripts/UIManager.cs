using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class UIManager : MonoBehaviour
{
    public Image damageIndicator;
    public Color damageColor;

    public static UIManager instance;
    //public LevelManager LM;

    public Player player;
    public TextMeshProUGUI playerHPTxt;
    public TextMeshProUGUI playerSPTxt;

    public GameObject gameOverCanvas;
    BoxerAIEnemy BoxerAIEnemy;


    public TextMeshProUGUI comboTxt;
    public TextMeshProUGUI SuperAttack;

    public GameObject effects_InputReadingArrowL; 
    public GameObject effects_InputReadingArrowR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        instance = this;
        //LM = gameObject.AddComponent<LevelManager>();
        BoxerAIEnemy = FindFirstObjectByType<BoxerAIEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        damageIndicator.color = damageColor;
        damageColor.a -= 0.1f;

        playerHPTxt.text = "HP: " + player.GetHP();
        playerSPTxt.text = "SP: " + player.GetSP();

        if(Player.instance.comboTimer > 0)
        {
            Player.instance.comboTimer--;
            if(Player.instance.combo > 1)
            {
                comboTxt.text = "Combo!: " + Player.instance.combo + "x";
            }
            
        }
        else
        {
            Player.instance.combo = 0;
            comboTxt.text = "";
        }

        if (BoxerAIEnemy.superAttack)
        {
            SuperAttack.enabled = true;
        }
        else
        {
            SuperAttack.enabled = false;
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


}
