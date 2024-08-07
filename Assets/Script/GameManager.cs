using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Wave> waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 10f;
    private int currentWaveIndex = 0;
    private bool isSpawningWave = false;

    public int playerHealth = 100;
    public int playerGold = 50;
    public Text healthText;
    public Text goldText;

    public Text waveText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnWave(currentWaveIndex)); // Bắt đầu với wave đầu tiên ngay khi game bắt đầu
    }

    public void UpdateUI()
    {
        healthText.text = "Health: " + playerHealth;
        goldText.text = "Gold: " + playerGold;
        waveText.text = "Wave: " + (currentWaveIndex + 1);
    }

    private IEnumerator SpawnWave(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartCoroutine(SpawnEnemies(waves[waveIndex]));
            currentWaveIndex = waveIndex; // Cập nhật chỉ số wave hiện tại
            UpdateUI();
        }
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        isSpawningWave = true;
        foreach (var enemy in wave.enemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
        isSpawningWave = false;

        // Bắt đầu wave kế tiếp sau khi wave hiện tại kết thúc
        if (currentWaveIndex + 1 < waves.Count)
        {
            StartCoroutine(SpawnWave(currentWaveIndex + 1));
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            GameOver();
        }
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }
}

[System.Serializable]
public class Wave
{
    public List<GameObject> enemies;
    public float spawnInterval;
}
