using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderButton : MonoBehaviour //성별 선택 버튼
{
    private float stayDelay;
    private float stayedTime;
    private float criteriaTime;
    // Start is called before the first frame update
    private void Start()
    {
        stayDelay = 1.0f; //터치 대기시간
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stayedTime = 0f;
        criteriaTime = stayDelay;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            stayedTime += Time.deltaTime;
            if (stayedTime > criteriaTime)
            {
                criteriaTime += stayDelay;
                
                GameObject.Find("GenderChat").SetActive(false);

                if (gameObject.name == "PrincessButton") // 선택된 버튼이 무엇인지에 따라
                {
                    GlobalManager.gender = false;
                    GlobalManager.UI.transform.Find("FemalePhase").gameObject.SetActive(true);
                    GameObject.Find("Chat").transform.Find("FemaleChat").gameObject.SetActive(true);
                }
                else
                {
                    GlobalManager.gender = true;
                    GlobalManager.UI.transform.Find("MalePhase").gameObject.SetActive(true);
                    GameObject.Find("Chat").transform.Find("MaleChat").gameObject.SetActive(true);
                }
                transform.parent.gameObject.SetActive(false); // 버튼 모음 해제
            }
        }
    }
}
