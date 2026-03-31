using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPosition;
    BoxerAIEnemy[] currentEnemies;
    Quaternion spawnRotation = Quaternion.Euler(0, 90, 0);
    [SerializeField] private TextMeshProUGUI scoreboard;
    private float totalPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalPoints = 0;
        spawnPosition = transform;
        scoreboard = FindFirstObjectByType<TextMeshProUGUI>();
        scoreboard.text = "Score: " + totalPoints.ToString();
    }

    void AddPoints(float p)
    {
        totalPoints += p;
    }
    void spawnEnemy()
    {
        if (spawnPosition == null)
        {
            return;
        }
        GameObject newEnemy = Instantiate(enemy, spawnPosition.localPosition, spawnRotation);
        Debug.Log("New boxer spawned in");
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3f);
        LevelManager.instance.FinishCombat();
        //spawnEnemy();
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        currentEnemies = FindObjectsByType<BoxerAIEnemy>(FindObjectsSortMode.None);
        if(currentEnemies.Length == 0)
        {
            
            Debug.Log("New enemy spawning in 3...2...1...");
            StartCoroutine(Cooldown());
        }
        scoreboard.text = "Score: "+totalPoints.ToString();
    }
}
