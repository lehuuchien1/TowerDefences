using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName; // Tên của enemy
    public float health;
    public float speed;
    public float damage;
    public float attackSpeed;
    public SkillData[] skills; // Danh sách các kỹ năng của enemy
}
