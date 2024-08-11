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
    protected float moveSpeed;
    protected float attackRadius;
    protected float originalMoveSpeed;
    protected float slowEffectMultiplier = 1f;

    protected Animator animator;
    protected bool isDead = false;
    protected Transform[] waypoints;
    protected int currentWaypointIndex = 0;
    protected float hitTimer = 0f;

    protected bool flipX = false;
    protected Transform targetSoldier;
    protected bool isAttacking = false;

    private float attackCooldown;
    private float attackCooldownTimer;

    public Transform[] Waypoints
    {
        get { return waypoints; }
        set { waypoints = value; }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    protected virtual void Start()
    {
        if (enemyData != null)
        {
            maxHealth = enemyData.health; // Khởi tạo maxHealth từ EnemyData
            health = maxHealth;
            moveSpeed = enemyData.speed;
            originalMoveSpeed = moveSpeed;
            attackRadius = enemyData.attackRadius;

            attackCooldown = 1f / enemyData.attackSpeed; // Tính toán thời gian giữa các đợt tấn công
            attackCooldownTimer = 0f;
        }

        animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            healthSlider.gameObject.SetActive(false);
        }

        waypoints = WaypointsManager.Instance.GetWaypoints();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        HandleHealthSlider();

        if (targetSoldier != null)
        {
            if (Vector2.Distance(transform.position, targetSoldier.position) < attackRadius)
            {
                AttackTarget();
            }
            else
            {
                ClearTarget();
            }
        }
        else
        {
            FindNearestSoldier();
            if (targetSoldier != null)
            {
                isAttacking = true;
                AttackTarget();
            }
            else
            {
                isAttacking = false;
                MoveToWaypoint();
            }
        }
    }

    protected virtual void FindNearestSoldier()
    {
        Soldier[] soldiers = FindObjectsOfType<Soldier>();
        float minDistance = attackRadius;
        Transform nearestSoldier = null;

        foreach (Soldier soldier in soldiers)
        {
            if (soldier.CompareTag("LinhCan"))
            {
                float distance = Vector2.Distance(transform.position, soldier.transform.position);
                if (distance < minDistance)
                {
                    nearestSoldier = soldier.transform;
                    minDistance = distance;
                }
            }
        }

        targetSoldier = nearestSoldier;
    }

    protected virtual void AttackTarget()
    {
        if (targetSoldier != null && attackCooldownTimer <= 0f)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Soldier soldier = targetSoldier.GetComponent<Soldier>();
            if (soldier != null)
            {
                soldier.SoldierHit(enemyData.damage); // Gửi sát thương từ EnemyData
            }

            attackCooldownTimer = attackCooldown; // Đặt lại thời gian giữa các đợt tấn công
        }
        else
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    protected virtual void HandleHealthSlider()
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
            float distanceThisFrame = moveSpeed * slowEffectMultiplier * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceThisFrame);

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
                if (currentWaypointIndex == waypoints.Length - 1)
                {
                    HandleReachedFinalWaypoint();
                }

                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                animator.SetBool("Run", true);
            }
        }
    }

    protected virtual void Flip()
    {
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
            Die();
        }
        UpdateHealthSlider();

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            hitTimer = sliderDisplayDuration;
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject);
        }
        StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
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

    public virtual void ApplySlow(float slowEffect, float duration)
    {
        slowEffectMultiplier = 1 - slowEffect;
        StartCoroutine(RemoveSlowEffectAfterDuration(duration));
    }

    protected virtual IEnumerator RemoveSlowEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        slowEffectMultiplier = 1;
    }

    protected virtual void HandleReachedFinalWaypoint()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage((int)enemyData.damage);
        }
        Destroy(gameObject);
    }

    public virtual void SetTargetSoldier(Transform newTarget)
    {
        targetSoldier = newTarget;
    }

    public virtual void ClearTarget()
    {
        targetSoldier = null;
    }
}
