using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class taikhoan : MonoBehaviour
{
    private string user = "lily";
    private string passwd = "lily123@";

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DangKy());
    }

    private IEnumerator DangKy()
    {
        WWWForm dataForm = new WWWForm();
        dataForm.AddField("user", user);
        dataForm.AddField("passwd", passwd);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", dataForm);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            print("Kết nối không thành công");
        }
        else if (www.isDone)
        {
            string get = www.downloadHandler.text;
            print(get);
        }
    }

    private IEnumerator DangNhap()
    {
        WWWForm dataForm = new WWWForm();
        dataForm.AddField("user", user);
        dataForm.AddField("passwd", passwd);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangnhap.php", dataForm);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            print("Kết nối không thành công");
        }
        else if (www.isDone)
        {
            string get = www.downloadHandler.text;
            print(get);
        }
    }
}