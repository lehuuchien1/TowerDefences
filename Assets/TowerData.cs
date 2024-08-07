using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDefense/TowerData", order = 2)]
public class TowerData : ScriptableObject
{
    public TowerLevelData[] levels; // Dữ liệu cấp độ của tháp
}

[System.Serializable]
public class TowerLevelData
{
    public float attackRadius;
    public float fireRate;
    public ProjectileData projectileData;
    public float spawnInterval;
    public int maxSoldiers;
    public float slowEffect;
    public float poisonDamage;
    public float poisonDuration;
    public int cost; // Giá của tháp
}

[System.Serializable]
public class ProjectileData
{
    public float damage;
    public float speed;
}
