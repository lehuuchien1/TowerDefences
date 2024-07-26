using UnityEngine;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    public Text waveText; // Text component để hiển thị wave

    private int currentWave = 0;

    public void UpdateWave(int wave)
    {
        currentWave = wave;
        waveText.text = "Wave: " + currentWave;
    }
}
