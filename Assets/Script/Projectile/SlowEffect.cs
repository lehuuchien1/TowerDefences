using System.Collections;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public float slowEffect = 0.5f; // Tỉ lệ làm chậm
    public float duration = 5f; // Thời gian tồn tại của vùng làm chậm

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplySlow(slowEffect, duration);
            }
        }
    }

    private void Start()
    {
        Destroy(gameObject, duration); // Hủy hiệu ứng sau thời gian tồn tại
    }
}
