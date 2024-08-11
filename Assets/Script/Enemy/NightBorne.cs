using System.Collections;
using UnityEngine;

public class NightBorne : Enemy
{
    // Thêm thuộc tính cho sát thương nổ và bán kính nổ
    public float explosionDamage = 50f;
    public float explosionRadius = 5f;

    // Ghi đè phương thức Hit để xử lý sát thương và kiểm tra trạng thái chết
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
            Explode();
            StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
        }

        UpdateHealthSlider();

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            hitTimer = sliderDisplayDuration;
        }
    }

    // Thêm phương thức Explode để gây sát thương lan
    private void Explode()
    {
        // Tìm tất cả các collider trong bán kính nổ
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Kiểm tra nếu collider thuộc loại Soldier và không phải là NightBorne
            Soldier soldier = hitCollider.GetComponent<Soldier>();
            if (soldier != null && soldier.gameObject != this.gameObject) // Loại trừ tổn thương tự gây ra
            {
                soldier.SoldierHit(explosionDamage);
            }
        }
    }


    // Phương thức này sẽ vẽ một hình tròn để dễ dàng kiểm tra bán kính nổ trong Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
