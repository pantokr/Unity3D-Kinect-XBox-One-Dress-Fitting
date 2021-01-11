using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpRecognizer : MonoBehaviour
{
    private GameObject joint;
    private GameObject footLeft;
    private GameObject footRight;

    private Vector3 firstFootLeftPos; //진입 발 위치 벡터
    private Vector3 firstFootRightPos;

    private Vector3 liveFootLeftPos; //실시간 발 위치 벡터
    private Vector3 liveFootRightPos;

    private readonly float jumpHeight = 50f;

    // Start is called before the first frame update
    private void Start()
    {
        footLeft = transform.Find("FootLeft").gameObject;
        footRight = transform.Find("FootRight").gameObject;

        firstFootLeftPos = footLeft.transform.localPosition;
        firstFootRightPos = footRight.transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        liveFootLeftPos = footLeft.transform.localPosition;
        liveFootRightPos = footRight.transform.localPosition;

        if (liveFootLeftPos.y > firstFootLeftPos.y + jumpHeight && liveFootRightPos.y > firstFootRightPos.y + jumpHeight)
        { //실시간 발 위치 벡터가 시작 발 위치 벡터보다 높이가 50 위일 때
            //Scene Changing
            SceneEnter(); 
        }
    }

    private void SceneEnter() // 점프 후 실행할 것들
    {
        GlobalManager.hasEntered = true;
        GlobalManager.entrant = gameObject.name;

        GameObject.Find("Entry").GetComponent<Entry>().enabled = true;

        DeleteBodies();

        Debug.Log("Jump is Recognized");
        Destroy(this);
    }

    private void DeleteBodies()
    {
        foreach(var i in JointRecognizer._Bodies) // Joint Recognizer 의 리스트랑 Hierachy의 오브젝트 삭제
        {
            if(i.Value == gameObject)
            {
                continue;
            }
            JointRecognizer._Bodies.Remove(i.Key);
        }

        GameObject bodies = GameObject.Find("Bodies");
        for (int i = 0; i < bodies.transform.childCount; i++)
        {
            if (bodies.transform.GetChild(i) == transform)
            {
                continue;
            }
        }
    }
}
