using UnityEngine;

[CreateAssetMenu(fileName = "SoldierData", menuName = "Soldier/SoldierData", order = 1)]
public class SoldierData : ScriptableObject
{
    public float health;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRadius;
    public float attackTarget;
}
