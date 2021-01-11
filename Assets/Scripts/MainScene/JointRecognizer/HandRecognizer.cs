using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class HandRecognizer : MonoBehaviour //손만 추출하는 클래스
{
    public Sprite Hand;
    private List<Kinect.JointType> _Hands = new List<Kinect.JointType>{
        Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight
     };
    // Start is called before the first frame update
    private void Start()
    {
        GameObject Hands = GameObject.Find("Hands");

        foreach (Kinect.JointType hand in _Hands)
        {
            GameObject tmp = Hands.transform.Find(hand.ToString()).gameObject;
            tmp.tag = "Hand";
            tmp.transform.localScale *= 0.5f;

            SpriteRenderer tmp_sr = tmp.GetComponent<SpriteRenderer>();

            if (hand == Kinect.JointType.HandRight) //손 좌우 반전 적용
            {
                tmp_sr.sprite = Hand;
            }
            else
            {
                tmp_sr.sprite = Hand;
                tmp_sr.flipX = true;
            }

            tmp.AddComponent<HandOption>(); //손에 스크립트 부착
        }
    }

    private void Update()
    {
        GameObject Hands = GameObject.Find("Hands");
        if (!Hands)
        {
            return;
        }

        foreach (Kinect.JointType hand in _Hands) //부드럽게 손이 이동하는 함수
        {
            
            GameObject tmp = Hands.transform.Find(hand.ToString()).gameObject; 
            GameObject joint = GameObject.Find(GlobalManager.entrant);

            Vector3 velo = Vector3.zero;
            tmp.transform.localPosition = Vector3.SmoothDamp(tmp.transform.localPosition, joint.transform.Find(hand.ToString()).localPosition, ref velo, 0.05f);
        }
    }
}