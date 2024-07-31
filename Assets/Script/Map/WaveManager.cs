using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves; // Mảng các wave
    public Transform spawnPoint; // Điểm spawn kẻ thù
    public float timeBetweenWaves = 5f; // Thời gian giữa các wave

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    private void Start()
    {
        if (waves.Length > 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        if (isSpawning) yield break;

        isSpawning = true;

        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.enemyDataArray.Length; i++)
        {
            EnemyData enemyData = currentWave.enemyDataArray[i];
            int count = currentWave.enemyCounts[i];

            for (int j = 0; j < count; j++)
            {
                SpawnEnemy(enemyData);
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }
        }

        isSpawning = false;

        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length)
        {
            currentWaveIndex = 0; // Lặp lại hoặc kết thúc wave
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave());
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        GameObject enemyPrefab = SelectEnemyPrefab(enemyData);
        if (enemyPrefab != null)
        {
            // Kiểm tra để đảm bảo spawnPoint không null
            if (spawnPoint != null)
            {
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Spawn point is not assigned!");
            }
        }
        else
        {
            Debug.LogError("No prefab found for enemy data: " + enemyData.name);
        }
    }

    private GameObject SelectEnemyPrefab(EnemyData enemyData)
    {
        Wave currentWave = waves[currentWaveIndex];
        for (int i = 0; i < currentWave.enemyDataArray.Length; i++)
        {
            if (currentWave.enemyDataArray[i] == enemyData)
            {
                return currentWave.enemyPrefabs[i];
            }
        }
        return null;
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }
}
