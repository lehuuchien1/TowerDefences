using System.Collections;
using UnityEngine;

public class ProductionTower : Tower
{
    public int goldPerInterval = 10; // Số vàng tăng thêm cho người chơi mỗi khoảng thời gian
    public float interval = 5f; // Khoảng thời gian (tính bằng giây) để tăng vàng

    public Color highlightColor = Color.yellow; // Màu sắc để làm nổi bật
    public float highlightDuration = 0.5f; // Thời gian làm nổi bật

    public GameObject goldAnimationPrefab; // Prefab cho animation vàng
    public float animationDuration = 0.5f; // Thời gian bay lên

    private float timer;
    private Renderer rend;
    private Color originalColor;

    protected override void Start()
    {
        base.Start();
        timer = interval; // Đặt timer để bắt đầu đếm ngược
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color; // Lưu màu sắc gốc
        }
    }

    protected override void Update()
    {
        base.Update();
        TimerUpdate();
    }

    private void TimerUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            AddGold();
            timer = interval; // Đặt lại timer
        }
    }

    private void AddGold()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(goldPerInterval);
            StartCoroutine(HighlightTower());
            StartCoroutine(PlayGoldAnimation());
        }
    }

    private IEnumerator HighlightTower()
    {
        if (rend != null)
        {
            rend.material.color = highlightColor; // Chuyển sang màu vàng
            yield return new WaitForSeconds(highlightDuration); // Chờ 0.5 giây
            rend.material.color = originalColor; // Khôi phục màu sắc gốc
        }
    }

    private IEnumerator PlayGoldAnimation()
    {
        if (goldAnimationPrefab != null)
        {
            // Instantiate gold animation
            GameObject goldAnimation = Instantiate(goldAnimationPrefab, transform.position, Quaternion.identity);

            // Fly up animation
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + Vector3.up * 2f; // Bay lên 2 đơn vị
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                goldAnimation.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            goldAnimation.transform.position = endPos;

            // Destroy gold animation
            Destroy(goldAnimation);
        }
    }

    protected override void Attack(Enemy enemy)
    {
        // ProductionTower không thực hiện tấn công, phương thức này có thể để trống hoặc không cần thiết
    }

    public override void OnProjectileDestroyed()
    {
        // ProductionTower không sử dụng projectile, phương thức này có thể để trống hoặc không cần thiết
    }
}
