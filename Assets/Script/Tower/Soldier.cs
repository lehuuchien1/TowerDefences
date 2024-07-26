using UnityEngine;

public class Soldier : MonoBehaviour
{
    public SoldierData soldierData;  // Tham chiếu đến ScriptableObject

    private float health;
    private float damage;
    private float moveSpeed;
    private float attackSpeed;
    private float attackRadius;

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
            health = soldierData.health;
            damage = soldierData.damage;
            moveSpeed = soldierData.moveSpeed;
            attackSpeed = soldierData.attackSpeed;
            attackRadius = soldierData.attackRadius;
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
            }

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (attackCooldownTimer <= 0f)
                {
                    Attack();
                    attackCooldownTimer = attackCooldown;
                }
                else
                {
                    attackCooldownTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, spawnPosition) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, spawnPosition, moveSpeed * Time.deltaTime);
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }
    }

    private void FlipTowardsTarget()
    {
        if (target == null) return;

        float direction = target.position.x - transform.position.x;
        if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (target != null)
        {
            target.GetComponent<Enemy>().Hit(damage);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void UpdateTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestEnemy = null;
        float minDistance = attackRadius;

        foreach (Enemy enemy in enemies)
        {
            // Kiểm tra trạng thái tàn hình của Wolf
            if (enemy is Wolf wolf && wolf.IsFading())
            {
                continue; // Bỏ qua đối tượng đang tàn hình
            }

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                closestEnemy = enemy.transform;
                minDistance = distance;
            }
        }

        target = closestEnemy;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
