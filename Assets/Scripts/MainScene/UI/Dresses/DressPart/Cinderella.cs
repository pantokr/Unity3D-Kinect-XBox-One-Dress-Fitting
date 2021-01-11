using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class Cinderella : DressFunctions
{
    private GameObject joint;
    private GameObject cinderella_whole;

    readonly Vector2Int colorResolution = new Vector2Int(1920, 1080);

    private void Start()
    {
        cinderella_whole = transform.Find("cinderella_whole").gameObject;
    }

    private void Update()
    {
        joint = GameObject.Find("Joint");
        if (joint)
        {
            DressFitting();
        }
    }

    private void DressFitting()
    {
        cinderella_whole_Fitting();
    }

    void cinderella_whole_Fitting()
    {
        Vector3 neckVector = joint.transform.Find(Kinect.JointType.Neck.ToString()).position;
       
        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;

        Vector3 footLeftVector = joint.transform.Find(Kinect.JointType.FootLeft.ToString()).position;
        Vector3 footRightVector = joint.transform.Find(Kinect.JointType.FootRight.ToString()).position;
        Vector3 footMiddleVector = middleVector(footLeftVector, footRightVector);

        float top = colorResolution.y / 2 - neckVector.y;
        float bottom = colorResolution.y / 2 + footMiddleVector.y;
        float left = colorResolution.x / 2 + shoulderLeftVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.9f;
        float right = colorResolution.x / 2 - shoulderRightVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.9f;

        cinderella_whole.GetComponent<RectTransform>().offsetMax = new Vector2(-right, -top);
        cinderella_whole.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);
        cinderella_whole.transform.rotation = angleCalculation(footMiddleVector, neckVector);
    }
}