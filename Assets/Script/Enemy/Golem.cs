using System.Collections;
using UnityEngine;

public class Golem : Enemy
{
    private bool skillActivated = false;

    protected override void Start()
    {
        base.Start();
        // Optionally initialize any additional Golem-specific properties here
    }

    public override void Hit(float damage)
    {
        if (health / maxHealth < 0.8f && !skillActivated)
        {
            ActivateDamageReductionSkill();
        }

        if (skillActivated)
        {
            damage *= 0.2f; // Apply damage reduction if the skill is active
        }

        health -= damage;

        if (health > 0)
        {
            // Ensure the Hit animation plays each time the Golem is hit
            PlayHitAnimation();
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

    private void ActivateDamageReductionSkill()
    {
        skillActivated = true;
        // You might want to add a skill duration or additional logic here
    }

    private void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit"); // Ensure the Hit animation plays
        }
    }

    // Animation Event method to reset the hit state
    private void OnAnimationHitEnd()
    {
        // Optionally reset hit state if needed
    }
}
