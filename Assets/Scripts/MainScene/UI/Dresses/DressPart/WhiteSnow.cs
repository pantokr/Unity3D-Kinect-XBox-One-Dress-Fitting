using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using Kinect = Windows.Kinect;

public class WhiteSnow : DressFunctions
{
    private GameObject joint;

    private GameObject whitesnow_body;
    private GameObject whitesnow_ls;
    private GameObject whitesnow_rs;
    private GameObject whitesnow_skirt;

    private readonly Vector2Int colorResolution = new Vector2Int(1920, 1080);

    private void Start()
    {
        whitesnow_body = transform.Find("whitesnow_body").gameObject;
        whitesnow_ls = transform.Find("whitesnow_ls").gameObject;
        whitesnow_rs = transform.Find("whitesnow_rs").gameObject;
        whitesnow_skirt = transform.Find("whitesnow_skirt").gameObject;
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
        whitesnow_body_Fitting();
        whitesnow_ls_Fitting();
        whitesnow_rs_Fitting();
        whitesnow_skirt_Fitting();
    }

    void whitesnow_body_Fitting()
    {
        Vector3 spineShoulderVector = joint.transform.Find(Kinect.JointType.SpineShoulder.ToString()).position;
        Vector3 spineBaseVector = joint.transform.Find(Kinect.JointType.SpineBase.ToString()).position;
        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;

        float top = colorResolution.y / 2 - spineShoulderVector.y - 50f;
        float bottom = colorResolution.y / 2 + spineBaseVector.y + 50f;
        float left = colorResolution.x / 2 + shoulderLeftVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.1f;
        float right = colorResolution.x / 2 - shoulderRightVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.1f;

        whitesnow_body.GetComponent<RectTransform>().offsetMax = new Vector2(-right, -top);
        whitesnow_body.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);
        whitesnow_body.transform.rotation = angleCalculation(spineBaseVector, spineShoulderVector);
    }
    void whitesnow_ls_Fitting()
    {
        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 elbowLeftVector = joint.transform.Find(Kinect.JointType.ElbowLeft.ToString()).position;

        whitesnow_ls.transform.localPosition = shoulderLeftVector;
        Vector3 angle = angleCalculation(elbowLeftVector, shoulderLeftVector).eulerAngles;
        angle.z += 10f;

        whitesnow_ls.transform.rotation = Quaternion.Euler(angle);

        float length = Vector3.Distance(shoulderLeftVector, elbowLeftVector);

        GameObject image = whitesnow_ls.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, length);
    }

    void whitesnow_rs_Fitting()
    {
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;
        Vector3 elbowRightVector = joint.transform.Find(Kinect.JointType.ElbowRight.ToString()).position;

        whitesnow_rs.transform.localPosition = shoulderRightVector;
        
        Vector3 angle = angleCalculation(elbowRightVector, shoulderRightVector).eulerAngles;
        angle.z -= 10f;

        whitesnow_rs.transform.rotation = Quaternion.Euler(angle);
        float length = Vector3.Distance(shoulderRightVector, elbowRightVector);

        GameObject image = whitesnow_rs.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, length);
    }

    void whitesnow_skirt_Fitting()
    {
        Vector3 spineBaseVector = joint.transform.Find(Kinect.JointType.SpineBase.ToString()).position;

        Vector3 ankleLeftVector = joint.transform.Find(Kinect.JointType.AnkleLeft.ToString()).position;
        Vector3 ankleRightVector = joint.transform.Find(Kinect.JointType.AnkleRight.ToString()).position;
        Vector3 ankleMiddleVector = middleVector(ankleLeftVector, ankleRightVector);

        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;

        float min_y = whitesnow_body.GetComponent<RectTransform>().offsetMin.y;

        float top = colorResolution.y - min_y;
        float bottom = colorResolution.y / 2 + ankleMiddleVector.y - 30f;
        float left = colorResolution.x / 2 + shoulderLeftVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.8f;
        float right = colorResolution.x / 2 - shoulderRightVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.9f;

        whitesnow_skirt.GetComponent<RectTransform>().offsetMax = new Vector2(-right, -top);
        whitesnow_skirt.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);

        whitesnow_skirt.transform.rotation = angleCalculation(ankleMiddleVector, spineBaseVector);
    }
}