using UnityEngine;

[CreateAssetMenu(fileName = "New Soldier Data", menuName = "Soldier Data",order =4)]
public class SoldierData : ScriptableObject
{
    public float health;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRadius;
}
