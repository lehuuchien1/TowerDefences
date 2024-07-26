using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    private float originalTimeScale = 1f;
    private bool isSpeedDoubled = false;

    void Start()
    {
        // Lưu giá trị Time.timeScale gốc
        originalTimeScale = Time.timeScale;
    }

    public void ToggleGameSpeed()
    {
        if (isSpeedDoubled)
        {
            // Đặt lại tốc độ game về giá trị gốc
            Time.timeScale = originalTimeScale;
        }
        else
        {
            // Tăng tốc độ game gấp đôi
            Time.timeScale = originalTimeScale * 2;
        }
        // Đảo ngược trạng thái
        isSpeedDoubled = !isSpeedDoubled;
    }
}
