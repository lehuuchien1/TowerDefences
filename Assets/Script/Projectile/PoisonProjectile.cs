using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float damage;
    private float speed;
    private Transform target;
    private PoisonTower poisonTower;
    public GameObject poisonEffectPrefab;

    private float poisonDamage;
    private float poisonDuration;

    public void Initialize(Transform _target, ProjectileData _projectileData, PoisonTower _poisonTower, float _poisonDamage, float _poisonDuration)
    {
        target = _target;
        poisonTower = _poisonTower;

        if (_poisonTower != null)
        {
            damage = _projectileData.damage;
            speed = _projectileData.speed;
            poisonDamage = _poisonDamage;
            poisonDuration = _poisonDuration;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (target == null)
        {
            DestroyProjectile();
            return;
        }

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (target != null && target.CompareTag("Enemy"))
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit(damage);

                if (poisonEffectPrefab != null)
                {
                    // Tạo vùng độc tại vị trí mục tiêu với thông tin từ projectile
                    GameObject poisonEffectGO = Instantiate(poisonEffectPrefab, target.position, Quaternion.identity);
                    PoisonEffect poisonEffect = poisonEffectGO.GetComponent<PoisonEffect>();
                    if (poisonEffect != null)
                    {
                        poisonEffect.Initialize(poisonDamage, poisonDuration);
                    }
                }
            }
        }

        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        if (poisonTower != null)
        {
            poisonTower.OnProjectileDestroyed();
        }
        Destroy(gameObject);
    }
}
