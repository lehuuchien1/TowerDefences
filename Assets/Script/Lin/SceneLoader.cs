using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static string sceneToLoad;

    public static void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public static AsyncOperation LoadTargetSceneAsync()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            return SceneManager.LoadSceneAsync(sceneToLoad);
        }
        return null;
    }
}
