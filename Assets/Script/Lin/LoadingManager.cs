using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    public Text progressText;

    void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    IEnumerator LoadTargetScene()
    {
        // Gọi phương thức LoadTargetSceneAsync từ SceneLoader
        AsyncOperation operation = SceneLoader.LoadTargetSceneAsync();

        // Kiểm tra nếu operation không null
        if (operation != null)
        {
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                if (progressBar != null)
                    progressBar.value = progress;
                if (progressText != null)
                    progressText.text = (progress * 100f).ToString("F2") + "%";

                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
