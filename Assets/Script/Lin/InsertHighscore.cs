using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class InsertHighscore : MonoBehaviour
{
    public TMP_InputField playerName;
    public TMP_InputField score;
    public TextMeshProUGUI thongbao;

    public void SaveHighscoreButton()
    {
        StartCoroutine(SaveHighscore());
    }

    IEnumerator SaveHighscore()
    {
        string token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(token))
        {
            thongbao.text = "Bạn cần đăng nhập để thực hiện thao tác này.";
            StartCoroutine(ClearMessageAfterDelay(3f));
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("playerName", playerName.text);
        form.AddField("score", score.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/InsertHighscore.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            thongbao.text = "Lỗi... không kết nối được server";
            StartCoroutine(ClearMessageAfterDelay(3f));
        }
        else
        {
            string response = www.downloadHandler.text;

            switch (response)
            {
                case "login":
                    thongbao.text = "Bạn cần đăng nhập để thực hiện thao tác này.";
                    break;
                case "Done":
                    thongbao.text = "Đã lưu dữ liệu lên server thành công.";
                    break;
                default:
                    thongbao.text = "Lỗi... không kết nối được server";
                    break;
            }
            StartCoroutine(ClearMessageAfterDelay(3f));
        }
    }

    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        thongbao.text = string.Empty;
    }
}
