using System.Collections;
using UnityEngine;

public class Cultist : Enemy
{
    public float skillInterval = 5.0f; // Thời gian chờ trước khi kích hoạt kỹ năng (tính bằng giây)
    public GameObject summonPrefab; // Prefab của đối tượng được triệu hồi
    public int summonCount = 3; // Số lượng đối tượng được triệu hồi mỗi lần

    protected override void Start()
    {
        base.Start(); // Gọi phương thức Start của lớp cha để khởi tạo waypoints và các thuộc tính khác

        // Bắt đầu Coroutine để kích hoạt kỹ năng
        StartCoroutine(ActivateSkillPeriodically());
    }

    private IEnumerator ActivateSkillPeriodically()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(skillInterval);
            ActivateSkill();
        }
    }

    private void ActivateSkill()
    {
        // Kích hoạt animation Summon
        if (animator != null)
        {
            animator.SetTrigger("Summon"); // Gọi trigger để phát animation Summon
        }

        // Thực hiện kỹ năng triệu hồi của Cultist ở đây
        Debug.Log("Cultist skill activated!");

        // Triệu hồi các đối tượng tại vị trí ngẫu nhiên xung quanh Cultist
        for (int i = 0; i < summonCount; i++)
        {
            Vector2 summonPosition = (Vector2)transform.position + Random.insideUnitCircle * 2.0f; // Vị trí ngẫu nhiên xung quanh Cultist
            Instantiate(summonPrefab, summonPosition, Quaternion.identity);
        }
    }

    public override void Hit(float damage)
    {
        if (isDead) return; // Prevent actions if already dead

        health -= damage;
        if (health > 0)
        {
            // Ensure Hit animation plays each time
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
        else
        {
            // Handle death
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
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
}
