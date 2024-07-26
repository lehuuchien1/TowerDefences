using UnityEngine;

public class FrostTower : Tower
{
    public GameObject frostProjectilePrefab; // Prefab của FrostProjectile
    public Transform firePoint;
    public GameObject slowEffectPrefab; // Prefab của vùng làm chậm

    private Transform target;
    private bool canShoot = true;
    private float frostTowerAttackCooldown = 0f;

    protected override void Update()
    {
        base.Update();

        if (frostProjectilePrefab == null || firePoint == null)
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

            if (frostTowerAttackCooldown <= 0f && canShoot)
            {
                Shoot();
                frostTowerAttackCooldown = 1f / towerData.levels[level].fireRate;
            }
        }

        frostTowerAttackCooldown -= Time.deltaTime;
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
        if (frostProjectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectileGO = Instantiate(frostProjectilePrefab, firePoint.position, firePoint.rotation);
        FrostProjectile frostProjectile = projectileGO.GetComponent<FrostProjectile>();

        if (frostProjectile != null)
        {
            frostProjectile.Initialize(target, towerData.levels[level].projectileData, this);
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
        // FrostTower sẽ không tấn công trực tiếp, mà sẽ bắn đạn
    }
}
