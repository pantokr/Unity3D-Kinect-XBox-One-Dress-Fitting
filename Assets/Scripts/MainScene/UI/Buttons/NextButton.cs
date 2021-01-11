using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    float stayedTime;
    float criteriaTime;
    float stayDelay;

    GameObject Slide;
    GameObject spObj;
    GameObject chat;

    Animator animator;

    private void Start()
    {
        stayDelay = 1.0f;

        Slide = GameObject.Find("Slide");
        spObj = GameObject.Find("ScenePhase1");
        chat = GameObject.Find("Chat");

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // 손과 충돌 시 
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime = 0f;
            criteriaTime = stayDelay;

            animator.Play("ButtonTouched"); //버튼 터치 애니메이션
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime += Time.deltaTime;
            if (stayedTime > criteriaTime)
            {
                criteriaTime += stayDelay;
                for (int i = 0; i < Slide.transform.childCount - 1; i++) // 0부터 마지막 - 1까지 다음 버튼 활성화
                {
                    GameObject del = Slide.transform.GetChild(i).gameObject;
                    if (del.activeSelf)
                    {
                        GameObject ins = Slide.transform.GetChild(i + 1).gameObject; // 드레스 파트 앞으로 이동
                        ins.SetActive(true);
                        del.SetActive(false);

                        //chat
                        chat.transform.Find("Button").gameObject.SetActive(false); //채팅 삭제 

                        if (GlobalManager.gender) //성별에 따라서 다른 채팅 삭제 및 활성화
                        {
                            chat.transform.Find("MaleChat").Find(ins.name).gameObject.SetActive(true);
                            chat.transform.Find("MaleChat").Find(del.name).gameObject.SetActive(false);
                        }
                        else
                        {
                            chat.transform.Find("FemaleChat").Find(ins.name).gameObject.SetActive(true);
                            chat.transform.Find("FemaleChat").Find(del.name).gameObject.SetActive(false);
                        }

                        if (i == 0) // 첫 번째 파트를 제외한 모든 파트에서 이전 버튼 활성화
                        {
                            transform.parent.Find("PreviousButton").gameObject.SetActive(true);
                        }

                        if (Slide.transform.GetChild(i + 1).gameObject.name == "AccesaryPart") // 마지막 파트에서 다음 버튼 비활성화 및 결정 버튼 활성화
                        {
                            //accesary
                            spObj.transform.Find("DecisionButton").gameObject.SetActive(true);
                            gameObject.SetActive(false);
                        }
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            animator.Rebind();
        }
    }
}
