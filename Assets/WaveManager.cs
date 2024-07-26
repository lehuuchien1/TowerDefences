using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class Wave
{
    public int[] enemyCounts; // Số lượng kẻ thù cho từng loại
    public float spawnInterval = 1f; // Thời gian giữa các kẻ thù trong wave
}

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 5 loại kẻ thù
    public Wave[] waves; // Danh sách các wave
    public Transform[] spawnPoints; // Các điểm spawn
    public float initialCountdown = 5f; // Thời gian chờ trước wave đầu tiên
    public float timeBetweenWaves = 5f; // Thời gian giữa các wave

    public TMP_Text waveText; // TextMeshPro để hiển thị số wave hiện tại
    public TMP_Text countdownText; // TextMeshPro để hiển thị thời gian đến wave tiếp theo

    private int currentWave = 0;
    private float countdownTimer;

    void Start()
    {
        countdownTimer = initialCountdown; // Bắt đầu với thời gian chờ trước wave đầu tiên
        UpdateWaveUI();
    }

    void Update()
    {
        if (currentWave < waves.Length)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                countdownTimer = 0;
                StartCoroutine(SpawnWaves());
                countdownTimer = timeBetweenWaves;
            }
            countdownText.text = "Time to next wave: " + Mathf.Ceil(countdownTimer).ToString();
        }
    }

    IEnumerator SpawnWaves()
    {
        if (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];
            Debug.Log("Wave " + (currentWave + 1) + " bắt đầu!");

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                int count = wave.enemyCounts[i];
                for (int j = 0; j < count; j++)
                {
                    SpawnEnemy(enemyPrefabs[i]);
                    yield return new WaitForSeconds(wave.spawnInterval);
                }
            }

            currentWave++;
            UpdateWaveUI();
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + (currentWave + 1).ToString();
        }
    }
}
