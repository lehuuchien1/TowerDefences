using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuaMan : MonoBehaviour
{
    public string tenManChoi;
    public void LoadManChoi()
    {
        SceneManager.LoadScene(tenManChoi);
    }
}
