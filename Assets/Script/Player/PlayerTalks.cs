using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerTalks : MonoBehaviour
{
    public Transform talksUI;//对话UI
    public Transform cueUI;//对话提示按钮
    //对话文本
    public Text texdialogueTex;
    //对话文字
    public List<string> texdialogue = new List<string>();
    //当前对话的索引
    private int dialogues;
    //对话按钮
    public Transform texdialogueBun;

    public Transform npc;//对话npc
    public Transform npcs;//攻击的npc

    public Transform BossUI;//Boss血条UI
    void Start()
    {
        //如果是Boss关卡
        if(SceneManager.GetActiveScene().name == "Boss")
        {
            GetComponent<Playerinps>().enabled = false;
            GetComponent<Players>().enabled = false;
            npc.GetComponent<Boss>().enabled = false;
            talksUI.gameObject.SetActive(true);
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
            Cursor.lockState = CursorLockMode.None;//鼠标显示
            texdialogueBun.GetComponent<Button>().onClick.AddListener(Onscenario);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Npc")
        {
            cueUI.gameObject.SetActive(true);

            //点击
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Playerinps>().enabled = false;
                GetComponent<Players>().enabled = false;
                talksUI.gameObject.SetActive(true);
                collision.transform.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));
                Cursor.lockState = CursorLockMode.None;//鼠标显示
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


    //对话按钮
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
                Cursor.lockState = CursorLockMode.Locked;//鼠标显示
            }
            if (SceneManager.GetActiveScene().name == "Level4")
            {
                talksUI.gameObject.SetActive(false);
                npc.GetComponent<Animator>().SetTrigger("Death");
                Destroy(npc,5);
                GetComponent<Playerinps>().enabled = true;
                GetComponent<Players>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;//鼠标显示
            }
            if (SceneManager.GetActiveScene().name == "Boss")
            {
                talksUI.gameObject.SetActive(false);
                GetComponent<Playerinps>().enabled = true;
                GetComponent<Players>().enabled = true;
                npc.GetComponent<Boss>().enabled = true;
                BossUI.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;//鼠标显示
            }
        }
        else
        {
            dialogues++;
            StartCoroutine(ShowTextCharacterByCharacter(texdialogue[dialogues]));

        }
    }
}
