using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private float poisonDamage;
    private float duration;

    public void Initialize(float _poisonDamage, float _duration)
    {
        poisonDamage = _poisonDamage;
        duration = _duration;
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(ApplyPoisonDamage(enemy));
            }
        }
    }

    private IEnumerator ApplyPoisonDamage(Enemy enemy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            enemy.Hit(poisonDamage * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
