using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerTalks : MonoBehaviour
{
    public Transform talksUI;//�Ի�UI
    public Transform cueUI;//�Ի���ʾ��ť
    //�Ի��ı�
    public Text texdialogueTex;
    //�Ի�����
    public List<string> texdialogue = new List<string>();
    //��ǰ�Ի�������
    private int dialogues;
    //�Ի���ť
    public Transform texdialogueBun;

    public Transform npc;//�Ի�npc
    public Transform npcs;//������npc

    public Transform BossUI;//BossѪ��UI
    void Start()
    {
        //�����Boss�ؿ�
        if(SceneManager.GetActiveScene().name == "Boss")
        {
            GetComponent<Playerinps>().enabled = false;
            GetComponent<Players>().enabled = false;
            npc.GetComponent<Boss>().enabled = false;
            talksUI.gameObject.SetActive(true);
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
            Cursor.lockState = CursorLockMode.None;//�����ʾ
            texdialogueBun.GetComponent<Button>().onClick.AddListener(Onscenario);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Npc")
        {
            cueUI.gameObject.SetActive(true);

            //���
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Playerinps>().enabled = false;
                GetComponent<Players>().enabled = false;
                talksUI.gameObject.SetActive(true);
                collision.transform.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
                Cursor.lockState = CursorLockMode.None;//�����ʾ
                texdialogueBun.GetComponent<Button>().onClick.AddListener(Onscenario);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Npc")
        {
            cueUI.gameObject.SetActive(false);
        }
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


    //�Ի���ť
    private void Onscenario()
    {
        if (dialogues >= texdialogue.Count - 1)
        {
            if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level3")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (SceneManager.GetActiveScene().name == "Level2")
            {
                talksUI.gameObject.SetActive(false);
                npc.gameObject.SetActive(false);
                npcs.gameObject.SetActive(true);
                GetComponent<Playerinps>().enabled = true;
                GetComponent<Players>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;//�����ʾ
            }
            if (SceneManager.GetActiveScene().name == "Level4")
            {
                talksUI.gameObject.SetActive(false);
                npc.GetComponent<Animator>().SetTrigger("Death");
                Destroy(npc,5);
                GetComponent<Playerinps>().enabled = true;
                GetComponent<Players>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;//�����ʾ
            }
            if (SceneManager.GetActiveScene().name == "Boss")
            {
                talksUI.gameObject.SetActive(false);
                GetComponent<Playerinps>().enabled = true;
                GetComponent<Players>().enabled = true;
                npc.GetComponent<Boss>().enabled = true;
                BossUI.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;//�����ʾ
            }
        }
        else
        {
            dialogues++;
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));

        }
    }
}
