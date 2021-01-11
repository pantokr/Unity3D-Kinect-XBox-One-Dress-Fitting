using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class CrownCommon : MonoBehaviour
{
    private GameObject joint;
    GameObject crown;
    // Start is called before the first frame update
    private void Start()
    {
        crown = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
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
        crown_Fitting();
    }

    private void crown_Fitting()
    {
        Vector3 headVector = joint.transform.Find(Kinect.JointType.Head.ToString()).position;
        headVector.y += 50f;
        crown.transform.localPosition = headVector;

        crown.transform.localScale = new Vector3(12f, 12f, 1f);
    }
}
