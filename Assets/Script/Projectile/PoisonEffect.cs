using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public float poisonDamage = 5f; // Sát thương độc
    public float duration = 5f; // Thời gian tồn tại của vùng độc

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
            enemy.Hit(poisonDamage * Time.deltaTime); // Gây sát thương độc liên tục
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Hủy hiệu ứng sau khi kết thúc
    }

    private void Start()
    {
        Destroy(gameObject, duration); // Hủy hiệu ứng sau thời gian tồn tại
    }
}
