using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using Kinect = Windows.Kinect;

public class Anna : DressFunctions
{
    private GameObject joint;

    private GameObject anna_body;
    private GameObject anna_ls;
    private GameObject anna_lh;
    private GameObject anna_rs;
    private GameObject anna_rh;
    private GameObject anna_skirt;

    private readonly Vector2Int colorResolution = new Vector2Int(1920, 1080);

    private void Start()
    {
        anna_body = transform.Find("anna_body").gameObject;
        anna_ls = transform.Find("anna_ls").gameObject;
        anna_lh = transform.Find("anna_lh").gameObject;
        anna_rs = transform.Find("anna_rs").gameObject;
        anna_rh = transform.Find("anna_rh").gameObject;
        anna_skirt = transform.Find("anna_skirt").gameObject;
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

        anna_body_Fitting();
        anna_ls_Fitting();
        anna_lh_Fitting();
        anna_rs_Fitting();
        anna_rh_Fitting();
        anna_skirt_Fitting();
    }

    void anna_body_Fitting()
    {
        Vector3 spineShoulderVector = joint.transform.Find(Kinect.JointType.SpineShoulder.ToString()).position;
        Vector3 spineBaseVector = joint.transform.Find(Kinect.JointType.SpineBase.ToString()).position;
        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;

        float top = colorResolution.y / 2 - spineShoulderVector.y - 50f;
        float bottom = colorResolution.y / 2 + spineBaseVector.y + 50f;
        float left = colorResolution.x / 2 + shoulderLeftVector.x; //- (shoulderRightVector.x - shoulderLeftVector.x) * 0.1f;
        float right = colorResolution.x / 2 - shoulderRightVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.1f;

        anna_body.GetComponent<RectTransform>().offsetMax = new Vector2(-right, -top);
        anna_body.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);
        anna_body.transform.rotation = angleCalculation(spineBaseVector, spineShoulderVector);
    }
    void anna_ls_Fitting()
    {
        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 elbowLeftVector = joint.transform.Find(Kinect.JointType.ElbowLeft.ToString()).position;

        anna_ls.transform.localPosition = shoulderLeftVector;
        anna_ls.transform.rotation = angleCalculation(elbowLeftVector, shoulderLeftVector);

        float length = Vector3.Distance(shoulderLeftVector, elbowLeftVector);

        GameObject image = anna_ls.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, length + 30f);
    }

    void anna_lh_Fitting()
    {
        Vector3 elbowLeftVector = joint.transform.Find(Kinect.JointType.ElbowLeft.ToString()).position;
        Vector3 wristLeftVector = joint.transform.Find(Kinect.JointType.WristLeft.ToString()).position;

        anna_lh.transform.localPosition = elbowLeftVector;
        anna_lh.transform.rotation = angleCalculation(wristLeftVector, elbowLeftVector);

        float length = Vector3.Distance(elbowLeftVector, wristLeftVector);

        GameObject image = anna_lh.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, length + 40f);
    }

    void anna_rs_Fitting()
    {
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;
        Vector3 elbowRightVector = joint.transform.Find(Kinect.JointType.ElbowRight.ToString()).position;

        anna_rs.transform.localPosition = shoulderRightVector;
        anna_rs.transform.rotation = angleCalculation(elbowRightVector, shoulderRightVector);

        float length = Vector3.Distance(shoulderRightVector, elbowRightVector);

        GameObject image = anna_rs.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, length + 30f);
    }

    void anna_rh_Fitting()
    {
        Vector3 elbowRightVector = joint.transform.Find(Kinect.JointType.ElbowRight.ToString()).position;
        Vector3 wristRightVector = joint.transform.Find(Kinect.JointType.WristRight.ToString()).position;

        anna_rh.transform.localPosition = elbowRightVector;
        anna_rh.transform.rotation = angleCalculation(wristRightVector, elbowRightVector);

        float length = Vector3.Distance(elbowRightVector, wristRightVector);

        GameObject image = anna_rh.transform.GetChild(0).gameObject;

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, length + 40f);
    }

    void anna_skirt_Fitting()
    {
        Vector3 spineBaseVector = joint.transform.Find(Kinect.JointType.SpineBase.ToString()).position;

        Vector3 ankleLeftVector = joint.transform.Find(Kinect.JointType.AnkleLeft.ToString()).position;
        Vector3 ankleRightVector = joint.transform.Find(Kinect.JointType.AnkleRight.ToString()).position;
        Vector3 ankleMiddleVector = middleVector(ankleLeftVector, ankleRightVector);

        Vector3 shoulderLeftVector = joint.transform.Find(Kinect.JointType.ShoulderLeft.ToString()).position;
        Vector3 shoulderRightVector = joint.transform.Find(Kinect.JointType.ShoulderRight.ToString()).position;

        float min_y = anna_body.GetComponent<RectTransform>().offsetMin.y;

        float top = colorResolution.y - min_y;
        float bottom = colorResolution.y / 2 + ankleMiddleVector.y - 30f;
        float left = colorResolution.x / 2 + shoulderLeftVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.8f;
        float right = colorResolution.x / 2 - shoulderRightVector.x - (shoulderRightVector.x - shoulderLeftVector.x) * 0.9f;

        anna_skirt.GetComponent<RectTransform>().offsetMax = new Vector2(-right, -top);
        anna_skirt.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom);

        anna_skirt.transform.rotation = angleCalculation(ankleMiddleVector, spineBaseVector);
    }
}