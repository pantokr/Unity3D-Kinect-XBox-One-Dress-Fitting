using UnityEngine;

public class Entry : MonoBehaviour
{
    private void Start() // 사람이 지목되자 마자 실행
    {
        StartUserTracker(); 
        StartHandRecognizer();
        StartChat();
    }

    private void StartUserTracker()
    {
        transform.Find("UserTracker").gameObject.SetActive(true);
    }

    private void StartHandRecognizer()
    {
        transform.Find("HandRecognizer").gameObject.SetActive(true);
    }

    private void StartChat()
    {
        GameObject.Find("UI").transform.Find("Chat").gameObject.SetActive(true);
    }
}
