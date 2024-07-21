using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Mảng các prefab của các loại quái vật
    public int[] enemyCounts; // Mảng số lượng quái vật tương ứng với từng loại

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Danh sách các quái vật đã được spawn

    void Start()
    {
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        // Kiểm tra xem số lượng loại quái vật và số lượng có bằng nhau không
        if (enemyPrefabs.Length != enemyCounts.Length)
        {
            Debug.LogError("Số lượng loại quái vật và số lượng không khớp nhau!");
            return;
        }

        // Spawn từng loại quái vật theo số lượng đã cho
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            for (int j = 0; j < enemyCounts[i]; j++)
            {
                Vector2 spawnPosition = transform.position; // Sử dụng vị trí của EnemySpawn
                GameObject enemy = Instantiate(enemyPrefabs[i], spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy); // Thêm quái vật vào danh sách
            }
        }
    }
}
