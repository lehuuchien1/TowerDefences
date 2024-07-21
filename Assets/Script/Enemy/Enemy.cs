using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private float health;
    private float maxHealth;
    private float moveSpeed;
    private bool isInvisible = false; // Biến để kiểm soát trạng thái tàng hình

    public Slider healthSlider;
    public float sliderDisplayDuration = 0.5f;
    private float hitTimer = 0f;
    private Animator animator;
    private bool isDead = false;
    public float invisibilityDuration = 2f; // tgian tanhinh
    public float Health { get { return health; } }
    public bool IsInvisible { get { return isInvisible; } } // Getter cho biến isInvisible

    void Start()
    {
        maxHealth = enemyData.hp;
        health = maxHealth;
        moveSpeed = enemyData.speed;

        animator = GetComponent<Animator>();
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            healthSlider.gameObject.SetActive(false);
        }

        StartCoroutine(ActivateInvisibility()); // Kích hoạt tàng hình khi mới xuất hiện
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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

        if (waypoints != null && waypoints.Length > 0 && !isDead)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (isDead) return;

        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
                animator.SetBool("Run", true);
            }
        }
    }

    public void Hit(float damage)
    {
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
                Destroy(healthSlider.gameObject); // Hủy thanh máu ngay lập tức
            }
            StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
        }
        UpdateHealthSlider();

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            hitTimer = sliderDisplayDuration;
        }

        animator.SetBool("Run", true);
    }

    IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    IEnumerator ActivateInvisibility()
    {
        isInvisible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f); // Làm kẻ thù tàng hình bằng cách giảm độ trong suốt
        }

        yield return new WaitForSeconds(invisibilityDuration); // Thay đổi thời gian tàng hình dựa trên biến

        isInvisible = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Khôi phục độ trong suốt
        }
    }
}
