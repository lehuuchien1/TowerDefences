using System.Collections;
using UnityEngine;

public class BarracksTower : Tower
{
    public GameObject soldierPrefab;  // Prefab của lính
    private float spawnInterval;  // Thời gian giữa các lần sinh lính
    private int maxSoldiers;  // Số lính tối đa
    private int currentSoldiers = 0;  // Số lính hiện tại

    [SerializeField] private float spawnRadius = 5f;  // Bán kính sinh lính

    protected override void Start()
    {
        base.Start();
        if (soldierPrefab == null)
        {
            return;  // Không cần thông báo lỗi
        }

        // Lấy các thuộc tính từ TowerData
        spawnInterval = towerData.levels[level].spawnInterval;
        maxSoldiers = towerData.levels[level].maxSoldiers;

        // Sinh ngay 2 lính khi bắt đầu
        for (int i = 0; i < 1; i++)
        {
            if (currentSoldiers < maxSoldiers)
            {
                SpawnSoldier();
                currentSoldiers++;
            }
        }

        // Bắt đầu Coroutine sinh lính
        StartCoroutine(SpawnSoldiers());
    }

    private IEnumerator SpawnSoldiers()
    {
        while (true)
        {
            if (currentSoldiers < maxSoldiers)
            {
                SpawnSoldier();
                currentSoldiers++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSoldier()
    {
        if (soldierPrefab == null)
        {
            return;  // Không cần thông báo lỗi
        }

        // Sinh vị trí ngẫu nhiên trong vùng spawnRadius quanh BarracksTower
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        // Tạo lính tại vị trí ngẫu nhiên
        GameObject soldier = Instantiate(soldierPrefab, randomPosition, Quaternion.identity);
        Soldier soldierScript = soldier.GetComponent<Soldier>();
        if (soldierScript == null)
        {
            return;  // Không cần thông báo lỗi
        }
        soldierScript.SetTarget(FindClosestEnemy());
        soldierScript.OnDestroyEvent += OnSoldierDestroyed;  // Đăng ký sự kiện OnDestroy
    }

    private Transform FindClosestEnemy()
    {
        // Tìm kiếm enemy gần nhất
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                closestEnemy = enemy.transform;
                minDistance = distance;
            }
        }
        return closestEnemy;
    }

    private void OnSoldierDestroyed()
    {
        currentSoldiers--;
    }

    protected override void Attack(Enemy enemy)
    {
        // Barracks không có hành vi tấn công trực tiếp, lính sẽ thực hiện tấn công
    }

    public override void OnProjectileDestroyed()
    {
        // BarracksTower không bắn đạn, nên không cần triển khai phương thức này
    }
}
