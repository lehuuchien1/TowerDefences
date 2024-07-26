﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DangNhapTaiKhoan : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI thongbao;

    public void DangNhapButton()
    {
        StartCoroutine(DangNhap());
    }

    IEnumerator DangNhap()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username.text);
        form.AddField("passwd", password.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangnhap.php", form);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            thongbao.text = "Kết nối không thành công";
        }
        else
        {
            string get = www.downloadHandler.text;

            if (get == "empty")
            {
                thongbao.text = "Vui lòng nhập thông tin đăng nhập";
            }
            else if (get == "" || get == null)
            {
                thongbao.text = "Tài khoản hoặc mật khẩu không chính xác";
            }
            else if (get.Contains("Lỗi"))
            {
                thongbao.text = "Không kết nối được server";
            }
            else
            {
                thongbao.text = "Đăng nhập thành công";
                PlayerPrefs.SetString("token", get);
                Debug.Log(get);
                yield return new WaitForSeconds(3);
                SceneManager.LoadScene("Chapter");
                yield break;
            }

            yield return new WaitForSeconds(3);
            thongbao.text = "";
        }
    }
}
