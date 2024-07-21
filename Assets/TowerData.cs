using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDefense/TowerData",order=2)]
public class TowerData : ScriptableObject
{
    public float attackRadius; // Bán kính tấn công
    public float speed; // Tốc độ bắn
    public float damage; // Sát thương của súng
}
