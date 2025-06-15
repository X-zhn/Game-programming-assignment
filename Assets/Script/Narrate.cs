using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Narrate : MonoBehaviour
{
    //对话文本
    public Text texdialogueTex;
    //对话文字
    private List<string> texdialogue = new List<string>();
    public List<string> texdialogue1;
    public List<string> texdialogue2;
    public List<string> texdialogue3;
    //当前对话的索引
    private int dialogues = 0;
    //对话按钮
    public Transform texdialogueBun;

    //图像
    public Image image;
    public List<Sprite> sprites;

    //音频
    public List<AudioClip> audioClips;
    public AudioSource audioSource;

    void Start()
    {
        if (Overall.progress == 0)
        {
            //初始介绍
            image.sprite = sprites[0];
            texdialogue = texdialogue1;
            audioSource.clip = audioClips[0];
        }
        if (Overall.progress == 1)
        {
            //胜利介绍
            image.sprite = sprites[1];
            texdialogue = texdialogue2;
            audioSource.clip = audioClips[1];
        }
        if (Overall.progress == 2)
        {
            //失败介绍
            image.sprite = sprites[2];
            texdialogue = texdialogue3;
            audioSource.clip = audioClips[2];
        }
        StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
        Cursor.lockState = CursorLockMode.None;//鼠标显示
        texdialogueBun.GetComponent<Button>().onClick.AddListener(Onscenario);
    }

    //对话文字一个一个出来
    IEnumerator ShowTextCharacterByCharacter(string text)
    {
        texdialogueTex.text = "";
        texdialogueBun.gameObject.SetActive(false);
        foreach (char c in text)
        {
            texdialogueTex.text += c;

            //释放按钮
            if (texdialogueTex.text == text)
            {
                texdialogueBun.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    //跳转场景
    private void Onscenario()
    {
        if (dialogues >= texdialogue.Count - 1)
        {
            if (Overall.progress == 0)
            {
                //进入第一关
                SceneManager.LoadScene("Level1");//返回主页
            }
            else
            {
                //返回主页
                SceneManager.LoadScene("Main");//返回主页
            }
        }
        else
        {
            dialogues++;
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));

        }
    }
}
