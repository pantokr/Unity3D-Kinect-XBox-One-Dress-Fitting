using System.Collections;
using UnityEngine;

public class Chat : MonoBehaviour // 채팅 시작 클래스
{
    private GameObject helloChat;
    private GameObject genderChat;

    // Start is called before the first frame update
    private void Start()
    {
        helloChat = transform.Find("HelloChat").gameObject; // 시작 챗
        genderChat = transform.Find("GenderChat").gameObject; //성별 선택 챗

        StartCoroutine(StartChat());
    }

    private IEnumerator StartChat()
    {
        helloChat.SetActive(true);

        yield return new WaitForSeconds(3.0f); //성별 선택은 시작 챗 실행 이후 3초 후

        helloChat.SetActive(false);
        genderChat.SetActive(true);

        GameObject.Find("UI").transform.Find("GenderButton").gameObject.SetActive(true);
    }
}
