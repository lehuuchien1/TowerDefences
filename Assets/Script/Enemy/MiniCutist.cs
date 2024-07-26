using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCutist : Enemy
{
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
