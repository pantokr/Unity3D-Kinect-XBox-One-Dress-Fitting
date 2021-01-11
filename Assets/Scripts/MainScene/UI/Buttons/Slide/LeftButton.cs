using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeftButton : MonoBehaviour
{
    float stayedTime;
    float criteriaTime;
    float stayDelay;

    Animator animator;

    readonly int OnSlideCount = 3; // 슬라이드 개수
    readonly List<Vector3> pos = new List<Vector3>() { // 각 슬라이드의 위치 리스트로 저장
        new Vector3(-100, 100, 0),
        new Vector3(0, 200, 0),
        new Vector3(100, 100, 0)
    };

    GameObject part; // 현재 파트
    int childCount;

    GameObject[] OnSlide; // 현재 슬라이드에 있는 옷 오브젝트 

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
        for (int i = 0; i < slide.transform.childCount - 2; i++) // 현재 슬라이드에 뭐가 있는지 저장
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
        for (int i = 0; i < part.transform.childCount; i++) // 현재 슬라이드에 있는 옷 오브젝트 위치 onslide 변수에 저장
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
                if (OnSlide[OnSlideCount - 1] == part.transform.GetChild(childCount - 1).gameObject) // 더 넘길 오브젝트가 슬라이드에 없으면 넘어가지 않음
                {
                    Debug.Log("End");
                }
                else
                {
                    for (int i = 0; i < part.transform.childCount - OnSlideCount - 1; i++) // 슬라이드 처음부터 순회
                    {
                        if (part.transform.GetChild(i).gameObject == OnSlide[0]) // 슬라이드의 현재 위치가 현재 보이는 슬라이드의 시작점과 일치한다면
                        {
                            OnSlide[0].SetActive(false); // 현재 보이는 슬라이드에서 첫번 째 (맨 왼쪽) 요소를 비활성화 한 뒤
                            for (int j = 0; j < OnSlideCount - 1; j++) // 그 위치에 현재 보여지는 슬라이드의 다른 요소들을 앞당김
                            {
                                OnSlide[j] = part.transform.GetChild(i + j + 1).gameObject;
                                OnSlide[j].transform.localPosition = pos[j];
                            }

                            OnSlide[OnSlideCount - 1] = part.transform.GetChild(i + OnSlideCount).gameObject; // 현재 보여지는 슬라이드의 마지막 요소에 알맞은 요소를 배치
                            OnSlide[OnSlideCount - 1].SetActive(true);

                            //animation

                            for (int j = 0; j < OnSlideCount; j++)
                            {
                                OnSlide[j].transform.GetChild(0).gameObject.GetComponent<Animator>().Rebind();
                            }

                            for (int j = 0; j < OnSlideCount; j++)
                            {
                                OnSlide[j].transform.GetChild(0).gameObject.GetComponent<Animator>().Play("Sprite_FadeIn");
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
