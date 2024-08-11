using System.Collections;
using UnityEngine;

public class NightBorne : Enemy
{
    public float explosionDamage = 50f;
    public float explosionRadius = 5f;
    public override void Hit(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health > 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
        else
        {
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
    private void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && enemy != this)
            {
                enemy.Hit(explosionDamage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
