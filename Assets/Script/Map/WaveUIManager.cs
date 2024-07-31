using UnityEngine;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    public WaveManager waveManager; // Tham chiếu đến WaveManager
    public Text waveText; // Text UI để hiển thị chỉ số wave

    private void Update()
    {
        if (waveManager != null && waveText != null)
        {
            int currentWaveIndex = waveManager.GetCurrentWaveIndex();
            waveText.text = "Wave: " + (currentWaveIndex + 1);
        }
    }
}
