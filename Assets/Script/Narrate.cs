using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Narrate : MonoBehaviour
{
    //�Ի��ı�
    public Text texdialogueTex;
    //�Ի�����
    private List<string> texdialogue = new List<string>();
    public List<string> texdialogue1;
    public List<string> texdialogue2;
    public List<string> texdialogue3;
    //��ǰ�Ի�������
    private int dialogues = 0;
    //�Ի���ť
    public Transform texdialogueBun;

    //ͼ��
    public Image image;
    public List<Sprite> sprites;

    //��Ƶ
    public List<AudioClip> audioClips;
    public AudioSource audioSource;

    void Start()
    {
        if (Overall.progress == 0)
        {
            //��ʼ����
            image.sprite = sprites[0];
            texdialogue = texdialogue1;
            audioSource.clip = audioClips[0];
        }
        if (Overall.progress == 1)
        {
            //ʤ������
            image.sprite = sprites[1];
            texdialogue = texdialogue2;
            audioSource.clip = audioClips[1];
        }
        if (Overall.progress == 2)
        {
            //ʧ�ܽ���
            image.sprite = sprites[2];
            texdialogue = texdialogue3;
            audioSource.clip = audioClips[2];
        }
        StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
        Cursor.lockState = CursorLockMode.None;//�����ʾ
        texdialogueBun.GetComponent<Button>().onClick.AddListener(Onscenario);
    }

    //�Ի�����һ��һ������
    IEnumerator ShowTextCharacterByCharacter(string text)
    {
        texdialogueTex.text = "";
        texdialogueBun.gameObject.SetActive(false);
        foreach (char c in text)
        {
            texdialogueTex.text += c;

            //�ͷŰ�ť
            if (texdialogueTex.text == text)
            {
                texdialogueBun.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    //��ת����
    private void Onscenario()
    {
        if (dialogues >= texdialogue.Count - 1)
        {
            if (Overall.progress == 0)
            {
                //�����һ��
                SceneManager.LoadScene("Level1");//������ҳ
            }
            else
            {
                //������ҳ
                SceneManager.LoadScene("Main");//������ҳ
            }
        }
        else
        {
            dialogues++;
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));

        }
    }
}
