using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float damage; // Sát thương của projectile
    private float speed; // Tốc độ di chuyển của projectile
    private Transform target;
    private PoisonTower poisonTower; // Thay GunTower bằng PoisonTower cho phù hợp

    public GameObject poisonEffectPrefab; // Prefab của vùng độc

    public void Initialize(Transform _target, ProjectileData _projectileData, PoisonTower _poisonTower)
    {
        target = _target;
        poisonTower = _poisonTower;

        if (_poisonTower != null)
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
                enemy.Hit(damage);

                if (poisonEffectPrefab != null)
                {
                    // Tạo vùng độc tại vị trí mục tiêu
                    GameObject poisonEffectGO = Instantiate(poisonEffectPrefab, target.position, Quaternion.identity);
                }
            }
        }

        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        if (poisonTower != null)
        {
            poisonTower.OnProjectileDestroyed(); // Gọi để cho phép tháp bắn lại
        }
        Destroy(gameObject);
    }
}
