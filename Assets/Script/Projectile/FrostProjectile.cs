using UnityEngine;

public class FrostProjectile : MonoBehaviour
{
    public float damage; // Sát thương của projectile
    private float speed; // Tốc độ di chuyển của projectile
    private Transform target;
    private FrostTower frostTower; // Thay GunTower bằng FrostTower cho phù hợp

    public GameObject slowEffectPrefab; // Prefab của vùng làm chậm

    public void Initialize(Transform _target, ProjectileData _projectileData, FrostTower _frostTower)
    {
        target = _target;
        frostTower = _frostTower;

        if (_frostTower != null)
        {
            damage = _projectileData.damage;
            speed = _projectileData.speed;
        }
        else
        {
            Destroy(gameObject); // Nếu có lỗi, hủy projectile ngay lập tức
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
                enemy.Hit(damage); // Gây sát thương cho enemy

                // Tạo SlowEffect tại vị trí của enemy
                if (slowEffectPrefab != null)
                {
                    Instantiate(slowEffectPrefab, enemy.transform.position, Quaternion.identity);
                }
            }
        }

        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        if (frostTower != null)
        {
            frostTower.OnProjectileDestroyed(); // Gọi để cho phép tháp bắn lại
        }
        Destroy(gameObject);
    }
}
