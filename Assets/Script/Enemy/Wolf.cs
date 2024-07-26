using System.Collections;
using UnityEngine;

public class Wolf : Enemy
{
    public float fadeDuration = 2f; // Thời gian tàn hình
    private bool isFading = false;
    private Renderer wolfRenderer;
    private Color initialColor;

    protected override void Start()
    {
        base.Start();
        wolfRenderer = GetComponent<Renderer>();

        if (wolfRenderer != null)
        {
            initialColor = wolfRenderer.material.color; // Lưu màu sắc ban đầu
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        // Bắt đầu với màu sắc đầy đủ
        wolfRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

        // Tàn hình dần dần
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            wolfRenderer.material.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo màu sắc cuối cùng là hoàn toàn tàn hình
        wolfRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        // Đợi một thời gian trước khi hiện lại
        yield return new WaitForSeconds(fadeDuration);

        // Hiện lại hình ảnh
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            wolfRenderer.material.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo màu sắc cuối cùng là hoàn toàn hiện rõ
        wolfRenderer.material.color = initialColor;

        isFading = false;
    }

    public bool IsFading()
    {
        return isFading; // Thêm phương thức công khai để kiểm tra trạng thái tàn hình
    }

    public override void Hit(float damage)
    {
        if (isDead || isFading) return;

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
