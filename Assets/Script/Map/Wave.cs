using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Game/Wave", order = 7)]
public class Wave : ScriptableObject
{
    public EnemyData[] enemyDataArray; // Danh sách dữ liệu của kẻ thù
    public int[] enemyCounts; // Số lượng kẻ thù mỗi loại
    public float spawnInterval; // Thời gian giữa các lần spawn kẻ thù
    public GameObject[] enemyPrefabs; // Các prefabs của kẻ thù
    public float waveDuration; // Thời gian cho mỗi wave
}
