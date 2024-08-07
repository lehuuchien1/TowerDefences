using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public TowerData towerData; // Dữ liệu tháp
    public int level; // Cấp độ của tháp
    public int cost => towerData.levels[level].cost; // Giá của tháp ở cấp độ hiện tại
    protected float attackCooldown; // Thời gian hồi phục sau mỗi lần bắn
    protected SpriteRenderer spriteRenderer;
    protected bool isPlaced = false; // Trạng thái của tháp (cần thay đổi từ private sang protected)

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCooldown = 1f / towerData.levels[level].fireRate;
    }

    protected virtual void Update()
    {
        if (!isPlaced)
            return;

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            AttackNearestEnemy();
            attackCooldown = 1f / towerData.levels[level].fireRate;
        }
    }

    protected abstract void Attack(Enemy enemy);

    protected void AttackNearestEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, towerData.levels[level].attackRadius);
        Enemy nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = hit.GetComponent<Enemy>();
                }
            }
        }

        if (nearestEnemy != null)
        {
            Attack(nearestEnemy);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.levels[level].attackRadius);
    }

    public abstract void OnProjectileDestroyed(); // Phương thức để gọi khi projectile bị hủy

    public virtual void DestroyTower()
    {
        Destroy(gameObject); // Xóa đối tượng tháp
    }

    public void PlaceTower()
    {
        isPlaced = true; // Đặt trạng thái tháp đã được đặt
    }
}
