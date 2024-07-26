using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage; // Sát thương của projectile
    private float speed; // Tốc độ di chuyển của projectile
    private Transform target;
    private GunTower gunTower;

    public GameObject explosionEffect; // Tham chiếu đến hiệu ứng nổ

    public void Initialize(Transform _target, ProjectileData _projectileData, GunTower _gunTower)
    {
        target = _target;
        gunTower = _gunTower; // Lưu tham chiếu đến GunTower

        if (_gunTower != null)
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
            }
        }

        PlayExplosionEffect();
        DestroyProjectile();
    }

    void PlayExplosionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f); // Hủy hiệu ứng nổ sau 0.5 giây
        }
    }

    void DestroyProjectile()
    {
        if (gunTower != null)
        {
            gunTower.OnProjectileDestroyed(); // Gọi để cho phép tháp bắn lại
        }
        Destroy(gameObject);
    }
}
