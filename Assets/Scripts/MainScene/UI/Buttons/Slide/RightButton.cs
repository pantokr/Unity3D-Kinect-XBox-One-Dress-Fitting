using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RightButton : MonoBehaviour
{
    float stayedTime;
    float criteriaTime;
    float stayDelay;

    Animator animator;

    readonly int OnSlideCount = 3;
    readonly List<Vector3> pos = new List<Vector3>() { // Left Button 아이디어와 유사 
        new Vector3(-100, 100, 0),
        new Vector3(0, 200, 0),
        new Vector3(100, 100, 0)
    };

    GameObject part;
    int childCount;

    GameObject[] OnSlide;

    private void Start()
    {
        animator = GetComponent<Animator>();
        stayDelay = 1.0f;

        OnSlide = new GameObject[OnSlideCount];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //What is enabled
        GameObject slide = GameObject.Find("Slide");
        for (int i = 0; i < slide.transform.childCount - 2; i++)
        {
            GameObject tmp = slide.transform.GetChild(i).gameObject;
            if (tmp.activeSelf)
            {
                part = tmp;
                childCount = part.transform.childCount;
                break;
            }
        }

        //Save each position
        for (int i = 0; i < part.transform.childCount; i++)
        {
            GameObject tmp = part.transform.GetChild(i).gameObject;

            if (tmp.activeSelf)
            {
                for (int j = 0; j < pos.Count; j++)
                {
                    if (tmp.transform.localPosition == pos[j])
                    {
                        OnSlide[j] = tmp;
                    }
                }
            }
        }

        //Button delay
        stayedTime = 0f;
        criteriaTime = stayDelay;

        //Animation
        animator.Play("Scale");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime += Time.deltaTime;
            if (stayedTime > criteriaTime)
            {
                criteriaTime += stayDelay;
                if (OnSlide[0] == part.transform.GetChild(0).gameObject)
                {
                    Debug.Log("End");
                }
                else
                {
                    for (int i = 1; i < part.transform.childCount - OnSlideCount; i++) // 슬라이드 전체 순회
                    {
                        if (part.transform.GetChild(i).gameObject == OnSlide[0])
                        {
                            OnSlide[OnSlideCount - 1].SetActive(false);
                            for (int j = OnSlideCount - 1; j > 0; j--) // Left Button과 반대 방향으로 현재 보여지는 슬라이드를 순회 
                            {
                                OnSlide[j] = part.transform.GetChild(i + j - 1).gameObject; // Left Button과 반대 방향으로 끌어 당김
                                OnSlide[j].transform.localPosition = pos[j];
                            }
                            OnSlide[0] = part.transform.GetChild(i - 1).gameObject; // 현재 보여지는 슬라이드의 첫 번째 요소에 알맞은 오브젝트를 배치 
                            OnSlide[0].SetActive(true);

                            //animation
                            for (int j = 0; j < OnSlideCount; j++)
                            {
                                OnSlide[j].transform.GetChild(0).gameObject.GetComponent<Animator>().Rebind();
                            }

                            for (int j = 0; j < OnSlideCount; j++)
                            {
                                OnSlide[j].transform.GetChild(0).GetComponent<Animator>().Play("Sprite_FadeIn");
                            }

                            break;
                        }
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
