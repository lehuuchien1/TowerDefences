using UnityEngine;

public class Soldier : MonoBehaviour
{
    public SoldierData soldierData;

    private float soldierHealth;
    private float damage;
    private float moveSpeed;
    private float attackSpeed;
    private float attackRadius;
    private float attackTarget;

    private Transform target;
    private Vector2 spawnPosition;
    private Animator animator;
    private float attackCooldown;
    private float attackCooldownTimer;
    public event System.Action OnDestroyEvent;

    void Start()
    {
        if (soldierData != null)
        {
            soldierHealth = soldierData.health;
            damage = soldierData.damage;
            moveSpeed = soldierData.moveSpeed;
            attackSpeed = soldierData.attackSpeed;
            attackRadius = soldierData.attackRadius;
            attackTarget = soldierData.attackTarget;
        }
        spawnPosition = transform.position;
        animator = GetComponent<Animator>();
        attackCooldown = 1f / attackSpeed;
        attackCooldownTimer = 0f;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void Update()
    {
        if (target != null)
        {
            // Kiểm tra nếu kẻ thù nằm trong phạm vi attackTarget
            if (Vector2.Distance(transform.position, target.position) <= attackTarget)
            {
                FlipTowardsTarget();

                Vector2 direction = target.position - transform.position;
                bool isMoving = direction.magnitude > 0.1f;
                if (isMoving)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    animator.SetBool("Run", true);
                }
                else
                {
                    animator.SetBool("Run", false);
                    Attack();
                }
            }
            else
            {
                // Nếu kẻ thù không nằm trong phạm vi attackTarget, di chuyển về vị trí ban đầu
                animator.SetBool("Run", true);
                MoveToInitialPosition();
            }
        }
        else
        {
            // Nếu không có mục tiêu, di chuyển về vị trí ban đầu
            animator.SetBool("Run", true);
            MoveToInitialPosition();
        }
    }

    private void MoveToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, spawnPosition, moveSpeed * Time.deltaTime);
    }

    private void UpdateTarget()
    {
        if (target == null)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            float minDistance = attackTarget; // Tầm phát hiện enemy
            Transform nearestEnemy = null;

            foreach (Enemy enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    nearestEnemy = enemy.transform;
                    minDistance = distance;
                }
            }

            if (nearestEnemy != null)
            {
                SetTarget(nearestEnemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            SetTarget(enemy.transform);
            Attack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && target == enemy.transform)
        {
            SetTarget(null);
        }
    }

    private void FlipTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            if (direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
            else if (direction.x > 0 && transform.localScale.x < 0)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Attack()
    {
        if (target != null && attackCooldownTimer <= 0f)
        {
            if (Vector2.Distance(transform.position, target.position) <= attackRadius) // Kiểm tra tầm đánh
            {
                animator.SetTrigger("Attack");
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Hit(damage);
                }
                attackCooldownTimer = attackCooldown;
            }
        }
        else
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    public void SoldierHit(float damageAmount)
    {
        soldierHealth -= damageAmount;
        if (soldierHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        OnDestroyEvent?.Invoke();
        Destroy(gameObject, 0.5f);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
