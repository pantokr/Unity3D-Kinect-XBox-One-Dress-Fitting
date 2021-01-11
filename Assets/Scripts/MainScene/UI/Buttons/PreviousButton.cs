using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousButton : MonoBehaviour
{
    float stayedTime;
    float criteriaTime;
    float stayDelay;

    GameObject Slide;
    GameObject chat;

    Animator animator;

    private void Start()
    {
        stayDelay = 1.0f;

        Slide = GameObject.Find("Slide");
        chat = GameObject.Find("Chat");

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime = 0f;
            criteriaTime = stayDelay;

            animator.Play("ButtonTouched");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // 다음 버튼과 아이디어 유사
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime += Time.deltaTime;
            if (stayedTime > criteriaTime)
            {
                criteriaTime += stayDelay;
                for (int i = 1; i < Slide.transform.childCount; i++) // 첫 번째 파트를 제외한 모든 나머지 파트에서 이전 버튼 활성화
                {
                    GameObject del = Slide.transform.GetChild(i).gameObject;

                    if (del.activeSelf)
                    {
                        GameObject ins = Slide.transform.GetChild(i - 1).gameObject; // 이전파트 활성화
                        ins.SetActive(true);
                        del.SetActive(false);

                        //chat
                        chat.transform.Find("Button").gameObject.SetActive(false);

                        if (GlobalManager.gender) // 성별에 따라서 다른 채팅 활성화
                        {
                            chat.transform.Find("MaleChat").Find(ins.name).gameObject.SetActive(true);
                            chat.transform.Find("MaleChat").Find(del.name).gameObject.SetActive(false);
                        }
                        else
                        {
                            chat.transform.Find("FemaleChat").Find(ins.name).gameObject.SetActive(true);
                            chat.transform.Find("FemaleChat").Find(del.name).gameObject.SetActive(false);
                        }

                        if (i == Slide.transform.childCount - 1) // 마지막 씬에서는 다음 버튼을 활성화 시키고 결정 버튼 비활성화 
                        {
                            transform.parent.Find("NextButton").gameObject.SetActive(true);
                            GameObject.Find("DecisionButton").SetActive(false);
                        }
                        if (ins.gameObject.name == "DressPart") // 첫 번째 파트에서 이전 버튼 비활성화
                        {
                            gameObject.SetActive(false);
                        }

                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // 버튼에서 손이 나가면 애니메이션 비활성화
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            animator.Rebind();
        }
    }
}
