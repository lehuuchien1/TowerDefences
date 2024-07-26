using System.Collections;
using UnityEngine;

public class SlimeMonster : Enemy
{
    public GameObject smallSlimePrefab; // Prefab for smaller slime instances

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
            Split(); // Split into smaller slimes
        }

        UpdateHealthSlider();

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            hitTimer = sliderDisplayDuration;
        }
    }

    private void Split()
    {
        Vector3[] positions = new Vector3[3];
        Vector3 offset = new Vector3(0.5f, 0.3f, 0); // Distance between smaller slimes

        for (int i = 0; i < 3; i++)
        {
            positions[i] = transform.position + offset * i;
        }

        for (int i = 0; i < 3; i++)
        {
            // Create a smaller slime instance from prefab
            GameObject smallSlimeInstance = Instantiate(smallSlimePrefab, positions[i], Quaternion.identity);
            Slime smallSlime = smallSlimeInstance.GetComponent<Slime>();

            if (smallSlime != null)
            {
                // Set properties for the smaller slime instance
                smallSlime.health = smallSlime.maxHealth; // Reset health for small slime
                smallSlime.SetWaypoints(Waypoints); // Set waypoints for smaller slimes
            }
        }
    }
}
