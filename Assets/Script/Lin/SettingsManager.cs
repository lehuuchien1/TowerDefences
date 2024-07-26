using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;  // Kéo và thả panel cài đặt vào đây từ Inspector
    private float originalTimeScale;

    void Start()
    {
        // Lưu giá trị Time.timeScale gốc
        originalTimeScale = Time.timeScale;
        // Đảm bảo panel cài đặt bị ẩn khi game bắt đầu
        settingsPanel.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (settingsPanel.activeSelf)
        {
            // Nếu panel cài đặt đang hiển thị, đặt lại tốc độ game về giá trị gốc
            Time.timeScale = originalTimeScale;
            settingsPanel.SetActive(false);
        }
        else
        {
            // Nếu panel cài đặt không hiển thị, đặt tốc độ game về 0
            Time.timeScale = 0;
            settingsPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        // Chuyển đến scene "Chapter"
        SceneManager.LoadScene("Chapter");
    }

    public void ResumeGame()
    {
        // Khôi phục tốc độ game và ẩn panel cài đặt
        Time.timeScale = originalTimeScale;
        settingsPanel.SetActive(false);
    }
}
