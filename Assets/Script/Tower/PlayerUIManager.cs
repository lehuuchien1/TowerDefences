using UnityEngine;
using TMPro; // Sử dụng để làm việc với TextMeshPro

public class PlayerUIManager : MonoBehaviour
{
    public TextMeshProUGUI livesText; // Tham chiếu đến TextMeshProUGUI component trên UI
    public GameObject gameOverPanel; // Tham chiếu đến GameOverPanel
    public int initialLives = 10; // Số mạng khởi tạo của người chơi

    private int lives; // Số mạng hiện tại của người chơi

    private void Start()
    {
        lives = initialLives; // Khởi tạo số mạng
        UpdateLivesUI(); // Cập nhật UI lần đầu tiên
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Đảm bảo bảng thông báo không hiển thị khi bắt đầu
        }
        Time.timeScale = 1; // Đảm bảo trò chơi không bị dừng khi bắt đầu
    }

    // Phương thức giảm mạng khi nhận sát thương
    public void TakeDamage(int damage)
    {
        lives -= damage;

        if (lives <= 0)
        {
            lives = 0;
            // Hiển thị bảng thông báo khi mạng người chơi bằng 0
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            Time.timeScale = 0; // Dừng tất cả các hành động trong game
            Debug.Log("Game Over");
        }

        UpdateLivesUI();
    }

    // Cập nhật UI để hiển thị số mạng hiện tại
    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }
}
