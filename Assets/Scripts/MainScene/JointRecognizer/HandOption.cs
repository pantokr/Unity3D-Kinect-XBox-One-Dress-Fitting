using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOption : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //미수정 스크립트
    {
        if (collision.gameObject.CompareTag("Clothing")) // 의류 태그 오브젝트 터치 시
        {
            GameObject slide = GameObject.Find("Slide");
            GameObject part = null;

            for (int i = 0; i < slide.transform.childCount; i++) //present part
            {
                if (slide.transform.GetChild(i).gameObject.activeSelf)
                {
                    part = slide.transform.GetChild(i).gameObject;
                    break;
                }
            }

            //chat
            GameObject chat = GameObject.Find("Chat");

            if (GlobalManager.gender) //성별에 따라 다른 오브젝트 해제
            {
                GameObject tmp = chat.transform.Find("MaleChat").gameObject;
                for (int i = 0; i < tmp.transform.childCount; i++)
                {
                    tmp.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                GameObject tmp = chat.transform.Find("FemaleChat").gameObject;
                for (int i = 0; i < tmp.transform.childCount; i++)
                {
                    tmp.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (part.name == "AccesaryPart") //마지막 의류 파트
            {
                chat.transform.Find("Camera").gameObject.SetActive(true);
            }
            else
            {
                chat.transform.Find("Button").gameObject.SetActive(true);
            }

            GameObject partOfDresses = GameObject.Find("Dresses").transform.Find(part.name).gameObject;
            GameObject obj = partOfDresses.transform.Find(collision.gameObject.name).gameObject;
            Animator objAnim = GameObject.Find("NextButton").GetComponent<Animator>();

            if (obj.activeSelf) //already dress on
            {
                for (int i = 0; i < partOfDresses.transform.childCount; i++)
                {
                    partOfDresses.transform.GetChild(i).gameObject.SetActive(false);
                    objAnim.Rebind();
                }
            }
            else // dress off
            {
                for (int i = 0; i < partOfDresses.transform.childCount; i++)
                {
                    partOfDresses.transform.GetChild(i).gameObject.SetActive(false);
                }
                obj.SetActive(true);
                objAnim.Play("DressTouched");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Clothing")) //코루틴
        {
            StartCoroutine(RetouchDelay(collision.gameObject));
        }
    }

    IEnumerator RetouchDelay(GameObject gameObject) //재 터치까지 공백시간 적용
    {
        CircleCollider2D cc = gameObject.transform.GetComponent<CircleCollider2D>();
        cc.enabled = false;

        yield return new WaitForSeconds(1.0f);

        cc.enabled = true;
    }
}
