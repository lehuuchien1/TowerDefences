using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDefense/TowerData", order = 2)]
public class TowerData : ScriptableObject
{
    public TowerLevelData[] levels; // Dữ liệu cấp độ của tháp
}

[System.Serializable]
public class TowerLevelData
{
    public float attackRadius; // Bán kính tấn công
    public float fireRate; // Tốc độ bắn
    public ProjectileData projectileData; // Dữ liệu projectile
    public float spawnInterval; // Thời gian giữa các lần sinh lính (dành cho BarracksTower)
    public int maxSoldiers; // Số lính tối đa (dành cho BarracksTower)
    public float slowEffect; // Hiệu ứng làm chậm (dành cho FrostTower)
    public float poisonDamage; // Sát thương độc (dành cho PoisonTower)
    public float poisonDuration;
}

[System.Serializable]
public class ProjectileData
{
    public float damage; // Sát thương của projectile
    public float speed; // Tốc độ di chuyển của projectile
}
