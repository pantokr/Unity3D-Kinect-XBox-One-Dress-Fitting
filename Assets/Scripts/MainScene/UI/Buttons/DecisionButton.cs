using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour //결정 버튼
{
    public RuntimeAnimatorController ScreenshotImageAnimatorController;

    public static string nowString;
    string filename;

    float stayedTime;
    float criteriaTime;
    float stayDelay;

    float scaleConstant;
    double timerDelay;
    readonly int number = 3; //타이머 초 개수

    GameObject hands;
    GameObject sp1;

    private void Start()
    {
        hands = GameObject.Find("Hands");
        sp1 = GameObject.Find("ScenePhase1");

        timerDelay = 1.2f; //타이머 1초당 1.2초로 계산
        scaleConstant = 2f;

        stayDelay = 1.0f;

        gameObject.transform.SetAsLastSibling(); //가장 앞에 보이도록
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

                #region enable and disable 
                //다른 모든 버튼 비활성화
                GameObject slide = GameObject.Find("Slide");
                for (int i = 0; i < slide.transform.childCount; i++)
                {
                    slide.transform.GetChild(i).gameObject.SetActive(false);
                }
                sp1.transform.Find("LeftButton").gameObject.SetActive(false);
                sp1.transform.Find("RightButton").gameObject.SetActive(false);
                sp1.transform.Find("PreviousButton").gameObject.SetActive(false);
                //채팅도 비활성화
                GameObject chat = GameObject.Find("Chat");

                for (int i = 0; i < chat.transform.childCount; i++)
                {
                    chat.transform.GetChild(i).gameObject.SetActive(false);
                }
                // 결정 버튼의 충돌, 이미지 요소 비활성화
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                // 뷰파인더 오브젝트 활성화
                transform.Find("ViewFinder").gameObject.SetActive(true);
                // 손 비활성화
                for (int i = 0; i < 2; i++)
                {
                    hands.transform.GetChild(i).gameObject.SetActive(false);
                }

                #endregion

                StartCoroutine(Timer());
            }
        }
    }

    private IEnumerator Timer()
    {
        int t = number;
        for (int i = 0; i < number; i++) //타이머 작동 함수
        {
            string n = "N" + t.ToString();
            GameObject _NumberN = GameObject.Find("Numbers").transform.Find(n).gameObject;
            _NumberN.SetActive(true);
            yield return new WaitForSeconds((float)timerDelay);

            _NumberN.SetActive(false);
            t--;
        }

        transform.Find("ViewFinder").gameObject.SetActive(false);

        StartCoroutine(ScreenshotCapture()); //스크린샷 캡처
        StartCoroutine(ScreenshotImageOnDisplay()); // 스크린샷 화면에 띄우기
    }
    private IEnumerator ScreenshotCapture()
    {
        yield return new WaitForSeconds(0.5f); //해당 경로에 사진 저장
        nowString = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        if (!Directory.Exists(Application.dataPath + "/Screenshots"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Screenshots");
        }
        filename = Application.dataPath + "/Screenshots/" + nowString + ".png";
        ScreenCapture.CaptureScreenshot(filename);
    }

    private IEnumerator ScreenshotImageOnDisplay()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject ScreenshotImage = new GameObject("ScreenshotImage");
        ScreenshotImage.transform.SetParent(sp1.transform.Find("CapturedImage"));
        ScreenshotImage.transform.localScale = new Vector3(1.08f * scaleConstant, 1.92f * scaleConstant, 1f); // 스크린샷 비율
        ScreenshotImage.transform.localPosition = new Vector3(0f, 0f, 0f);

        ScreenshotImage.AddComponent<RawImage>();
        ScreenshotImage.AddComponent<Animator>();

        Animator ScreenshotImageAnimator = ScreenshotImage.GetComponent<Animator>();
        ScreenshotImageAnimator.runtimeAnimatorController = ScreenshotImageAnimatorController;

        GameObject OutlineImage = Instantiate(ScreenshotImage, sp1.transform.Find("CapturedImage"));
        OutlineImage.name = "OutlineImage";
        OutlineImage.transform.localScale = new Vector3(1.08f * scaleConstant + 0.2f, 1.92f * scaleConstant + 0.2f, 1f); // 외곽을 위한 이미지

        //Screenshot Image 파일 저장

        Texture2D texture = new Texture2D(0, 0);
        byte[] readByte = File.ReadAllBytes(filename);

        if (readByte.Length > 0)
        {
            texture.LoadImage(readByte);
        }

        ScreenshotImage.GetComponent<RawImage>().texture = texture;
        ScreenshotImage.transform.SetAsLastSibling();

        //Put dresses off and turn buttons on

        for (int i = 0; i < 2; i++)
        {
            hands.transform.GetChild(i).gameObject.SetActive(true);
        }

        DressOff();
        PrtButtonOn();
        BackButtonOn();
    }
    private void DressOff()
    {
        GameObject dresses = GameObject.Find("Dresses");
        dresses.SetActive(false);
    }
    private void PrtButtonOn()
    {
        GameObject prtButton = sp1.transform.Find("PrintButton").gameObject;
        prtButton.SetActive(true);
        prtButton.transform.SetAsLastSibling();
    }
    private void BackButtonOn()
    {
        GameObject homeButton = sp1.transform.Find("HomeButton").gameObject;
        homeButton.SetActive(true);
        homeButton.transform.SetAsLastSibling();
    }
}