using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIs : MonoBehaviour
{

    public Slider volumes;
    public static float volumef = 1;
    public Transform volumeTran;//“Ù¡øUI

    void Start()
    {
        volumes.value = volumef;
    }

    public void OnKai()
    {
        Overall.progress = 0;
        SceneManager.LoadScene("Plot");
    }

    public void Onjie()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

    void Update()
    {
        volumef = volumes.value;
    }

    public void Onjiaohu(bool isjiao)
    {
        if (isjiao)
        {
            volumeTran.gameObject.SetActive(true);
        }
        else
        {
            volumeTran.gameObject.SetActive(false);
        }
    }
}
