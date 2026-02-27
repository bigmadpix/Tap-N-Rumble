using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPosition;
    BoxerAIEnemy[] currentEnemies;
    Quaternion spawnRotation = Quaternion.Euler(0, 90, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition = transform;
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
        spawnEnemy();
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
    }
}
