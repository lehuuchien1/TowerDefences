using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public Slider healthSlider;
    public float sliderDisplayDuration = 0.3f;

    public float health;
    public float maxHealth;
    public float moveSpeed;
    protected Animator animator;
    protected bool isDead = false;
    protected Transform[] waypoints;
    protected int currentWaypointIndex = 0;
    protected float hitTimer = 0f;
    private float originalMoveSpeed; // Lưu tốc độ gốc
    private float slowEffectMultiplier = 1f; // Tỉ lệ làm chậm hiện tại

    // Thêm biến flipX
    private bool flipX = false;

    // Sát thương khi kẻ thù đến điểm waypoint cuối cùng
    public float damageToPlayer = 10f;

    public Transform[] Waypoints
    {
        get { return waypoints; }
        set { waypoints = value; }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0; // Đặt lại chỉ số waypoint khi cập nhật
    }

    protected virtual void Start()
    {
        maxHealth = enemyData.health;
        health = maxHealth;
        moveSpeed = enemyData.speed;
        originalMoveSpeed = moveSpeed; // Lưu tốc độ gốc

        animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            healthSlider.gameObject.SetActive(false);
        }

        waypoints = WaypointsManager.Instance.GetWaypoints();
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints set.");
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        HandleHealthSlider();

        if (!isDead)
        {
            MoveToWaypoint();
        }
    }

    private void HandleHealthSlider()
    {
        if (healthSlider != null && healthSlider.gameObject.activeSelf)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            healthSlider.transform.position = screenPosition + new Vector3(0, 50, 0);
        }

        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                if (healthSlider != null)
                {
                    healthSlider.gameObject.SetActive(false);
                }
            }
        }
    }

    protected virtual void MoveToWaypoint()
    {
        if (waypoints.Length > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            float distanceThisFrame = moveSpeed * slowEffectMultiplier * Time.deltaTime; // Nhân với tỉ lệ làm chậm

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceThisFrame);

            // Xác định hướng flip
            if (direction.x < 0 && !flipX)
            {
                Flip();
            }
            else if (direction.x > 0 && flipX)
            {
                Flip();
            }

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                Debug.Log("Checking final waypoint.");
                if (currentWaypointIndex == waypoints.Length - 1)
                {
                    Debug.Log("Reached final waypoint.");
                    HandleReachedFinalWaypoint();
                }

                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                animator.SetBool("Run", true);
            }
        }
    }

    private void Flip()
    {
        // Đảo ngược trục X
        flipX = !flipX;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public virtual void Hit(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            animator.SetTrigger("Die");
            isDead = true;
            if (healthSlider != null)
            {
                Destroy(healthSlider.gameObject);
            }
            StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
        }
        UpdateHealthSlider();

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            hitTimer = sliderDisplayDuration;
        }
    }

    protected virtual IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    protected virtual void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }

    public void ApplySlow(float slowEffect, float duration)
    {
        slowEffectMultiplier = 1 - slowEffect; // Cập nhật tỉ lệ làm chậm
        StartCoroutine(RemoveSlowEffectAfterDuration(duration)); // Xóa hiệu ứng sau một thời gian
    }

    private IEnumerator RemoveSlowEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        slowEffectMultiplier = 1; // Khôi phục tốc độ gốc
    }

    private void HandleReachedFinalWaypoint()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage((int)damageToPlayer);
        }
        Destroy(gameObject);
    }
}
