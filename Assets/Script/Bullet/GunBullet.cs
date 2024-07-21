using UnityEngine;

public class GunBullet : MonoBehaviour
{
    private float damage; // Sát thương của đạn, thiết lập từ GunTower
    private Transform target;
    private float speed; // Tốc độ sẽ được thiết lập từ GunTower
    private GunTower gunTower; // Tham chiếu đến GunTower

    public void Initialize(Transform _target, float _speed, GunTower _gunTower)
    {
        target = _target;
        speed = _speed;
        gunTower = _gunTower; // Lưu trữ tham chiếu đến GunTower
        damage = gunTower.stats.damage; // Gán sát thương từ GunTower
    }

    void Update()
    {
        if (target == null)
        {
            DestroyBullet();
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

        DestroyBullet();
    }

    void DestroyBullet()
    {
        if (gunTower != null)
        {
            gunTower.OnBulletDestroyed(); // Thông báo cho GunTower rằng đạn đã bị hủy
        }
        Destroy(gameObject);
    }
}
