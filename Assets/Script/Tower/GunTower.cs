using UnityEngine;

public class GunTower : Tower
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform target;
    private bool canShoot = true;
    private float gunTowerAttackCooldown = 0f; // Đổi tên biến

    protected override void Update()
    {
        base.Update();

        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        if (target == null || Vector2.Distance(transform.position, target.position) > towerData.levels[level].attackRadius || TargetIsDead())
        {
            FindNearestEnemy();
        }

        if (target != null)
        {
            FlipTowardsTarget();

            if (gunTowerAttackCooldown <= 0f && canShoot)
            {
                Shoot();
                gunTowerAttackCooldown = 1f / towerData.levels[level].fireRate;
            }
        }

        gunTowerAttackCooldown -= Time.deltaTime;
    }

    private bool TargetIsDead()
    {
        if (target != null)
        {
            Enemy enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                return enemyComponent.health <= 0;
            }
        }
        return false;
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= towerData.levels[level].attackRadius)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.health > 0 && !(enemyComponent is Wolf wolf && wolf.IsFading()))
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        target = nearestEnemy != null ? nearestEnemy.transform : null;
    }


    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(target, towerData.levels[level].projectileData, this);
        }

        canShoot = false;
    }

    public override void OnProjectileDestroyed()
    {
        canShoot = true;
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

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }

    protected override void Attack(Enemy enemy)
    {
        // GunTower sẽ không tấn công trực tiếp, mà sẽ bắn đạn
    }
}
