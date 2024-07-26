using UnityEngine;

public class PoisonTower : Tower
{
    public GameObject poisonProjectilePrefab; // Prefab của PoisonProjectile
    public Transform firePoint;
    public GameObject poisonEffectPrefab; // Prefab của vùng gây độc

    private Transform target;
    private bool canShoot = true;
    private float poisonTowerAttackCooldown = 0f;

    protected override void Update()
    {
        base.Update();

        if (poisonProjectilePrefab == null || firePoint == null)
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

            if (poisonTowerAttackCooldown <= 0f && canShoot)
            {
                Shoot();
                poisonTowerAttackCooldown = 1f / towerData.levels[level].fireRate;
            }
        }

        poisonTowerAttackCooldown -= Time.deltaTime;
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
        if (poisonProjectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectileGO = Instantiate(poisonProjectilePrefab, firePoint.position, firePoint.rotation);
        PoisonProjectile poisonProjectile = projectileGO.GetComponent<PoisonProjectile>();

        if (poisonProjectile != null)
        {
            poisonProjectile.Initialize(target, towerData.levels[level].projectileData, this);
        }

        if (poisonEffectPrefab != null)
        {
            GameObject poisonEffectGO = Instantiate(poisonEffectPrefab, firePoint.position, Quaternion.identity);
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
        // PoisonTower sẽ không tấn công trực tiếp, mà sẽ bắn đạn
    }
}
