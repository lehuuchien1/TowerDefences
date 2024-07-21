using UnityEngine;

public class GunTower : MonoBehaviour
{
    public TowerData stats; // Tham chiếu đến Scriptable Object chứa chỉ số
    public GameObject bulletPrefab; // Đạn bắn ra
    public Transform firePoint; // Điểm xuất phát của đạn

    private float fireCountdown = 0f;
    private Transform target;
    private bool canShoot = true; // Cờ kiểm tra liệu có thể bắn hay không
    private SpriteRenderer spriteRenderer; // Component để lật sprite

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (target == null || Vector2.Distance(transform.position, target.position) > stats.attackRadius || TargetIsDead())
        {
            FindNearestEnemy();
        }

        if (target != null)
        {
            FlipTowardsTarget(); // Lật trục X theo hướng của mục tiêu

            if (fireCountdown <= 0f && canShoot)
            {
                Shoot();
                fireCountdown = 1f / stats.speed; // Cập nhật lại dựa trên tốc độ bắn
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= stats.attackRadius)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.Health > 0 && !enemyComponent.IsInvisible)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        target = nearestEnemy != null ? nearestEnemy.transform : null;
    }

    bool TargetIsDead()
    {
        if (target != null)
        {
            Enemy enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent != null && enemyComponent.Health <= 0)
            {
                return true;
            }
        }
        return false;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GunBullet bullet = bulletGO.GetComponent<GunBullet>();

        if (bullet != null)
        {
            bullet.Initialize(target, stats.speed, this); // Truyền tham chiếu đến GunTower
        }

        canShoot = false; // Không thể bắn tiếp cho đến khi đạn hiện tại bị hủy
    }


    public void OnBulletDestroyed()
    {
        canShoot = true; // Cho phép bắn tiếp khi đạn hiện tại đã bị hủy
    }

    void FlipTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction.x > 0 && !spriteRenderer.flipX || direction.x < 0 && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRadius);
    }
}
