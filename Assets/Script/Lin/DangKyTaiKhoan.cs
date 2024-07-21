using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DangKyTaiKhoan : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI thongbao;

    public void DangKyButton()
    {
        StartCoroutine(DangKy());
    }
    IEnumerator DangKy()
    {
        WWWForm form = new WWWForm();
        form.AddField("user",username.text);
        form.AddField("passwd",password.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", form);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            thongbao.text = "Kết nối không thành công";
        }
        else
        {
            string get = www.downloadHandler.text;

            switch (get)
            {
                case "exist": thongbao.text = "Tài khoản đã tồn tài";break;
                case "OK": thongbao.text = "Đăng kí thành công"; break;
                case "ERROR": thongbao.text = "Đăng kí không thành công"; break;
                default: thongbao.text = "Sever đang bận";break;
            }
        }
    }
}
