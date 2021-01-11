using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class HeadArrow : MonoBehaviour // 사람 가리키는 함수
{
    // Update is called once per frame
    private void Start()
    {
    }

    private void Update()
    {
        GameObject bodyObject = GameObject.Find(GlobalManager.entrant);

        Vector3 objPoint = bodyObject.transform.Find(Kinect.JointType.Head.ToString()).transform.position;
        objPoint.y += 100f;
        objPoint.z = 0f;
        transform.localPosition = objPoint;
    }
}
