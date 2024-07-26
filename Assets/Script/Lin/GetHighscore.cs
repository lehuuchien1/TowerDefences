using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetHighscore : MonoBehaviour
{
    public TextMeshProUGUI highscoreList;

    public void LoadHighscoreButton()
    {
        StartCoroutine(LoadHighscore());
    }

    IEnumerator LoadHighscore()
    {
        string token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(token))
        {
            highscoreList.text = "Bạn cần đăng nhập để thực hiện thao tác này.";
            yield return new WaitForSeconds(3);
            highscoreList.text = "";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("token", token);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/GetHighscore.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            highscoreList.text = "Lỗi... không kết nối được server";
            yield return new WaitForSeconds(3);
            highscoreList.text = "";
        }
        else
        {
            string response = www.downloadHandler.text;

            if (string.IsNullOrEmpty(response))
            {
                highscoreList.text = "Không có dữ liệu";
                yield return new WaitForSeconds(3);
                highscoreList.text = "";
            }
            else
            {
                highscoreList.text = response.Replace("\n", "\n");
                yield return new WaitForSeconds(3);
                highscoreList.text = "";
            }
        }
    }
}
